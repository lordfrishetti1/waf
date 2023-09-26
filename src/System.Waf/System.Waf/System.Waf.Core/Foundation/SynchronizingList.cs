﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Represents a collection that synchronizes all of it's items with the items of the specified original collection.
    /// Supports two-way synchronization. Limitation: Add, Insert and SetItem can only be done on the original list.
    /// Uses weak events to prevent memory leaks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <typeparam name="TOriginal">The type of elements in the original collection.</typeparam>
    public class SynchronizingList<T, TOriginal> : ObservableList<T>
    {
        private readonly List<(TOriginal original, T newItem)> mapping = new();
        private readonly IEqualityComparer<T> itemComparer = EqualityComparer<T>.Default;
        private readonly IEqualityComparer<TOriginal> originalItemComparer = EqualityComparer<TOriginal>.Default;
        private readonly IEnumerable<TOriginal> originalCollection;
        private readonly ObservableCollection<TOriginal>? originalList;
        private readonly Func<TOriginal, T> factory;
        private readonly bool isReadOnly;
        private bool innerChange;

        /// <summary>Initializes a new instance of the <see cref="SynchronizingList{T, TOriginal}"/> class with two-way synchronization support.</summary>
        /// <param name="originalList">The original list.</param>
        /// <param name="factory">The factory which is used to create new elements in this collection.</param>
        /// <param name="isReadOnly">Set true to define a read-only synchronizing list, otherwise set false.</param>
        /// <exception cref="ArgumentNullException">The arguments originalCollection and factory must not be null.</exception>
        public SynchronizingList(ObservableCollection<TOriginal> originalList, Func<TOriginal, T> factory, bool isReadOnly = false) : this(originalList, originalList, factory, isReadOnly) { }

        /// <summary>Initializes a new instance of the <see cref="SynchronizingList{T, TOriginal}"/> class with one-way synchronization support. The instance is read-only.</summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <param name="factory">The factory which is used to create new elements in this collection.</param>
        /// <exception cref="ArgumentNullException">The arguments originalCollection and factory must not be null.</exception>
        public SynchronizingList(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory) : this(null, originalCollection, factory) { }

        private SynchronizingList(ObservableCollection<TOriginal>? originalList, IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory, bool isReadOnly = false)
        {
            this.originalList = originalList;
            this.originalCollection = originalCollection ?? throw new ArgumentNullException(nameof(originalCollection));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.isReadOnly = originalList is null || isReadOnly;

            INotifyCollectionChanged? observableCollection = originalList ?? originalCollection as INotifyCollectionChanged;
            if (observableCollection is not null) WeakEvent.CollectionChanged.Add(observableCollection, OriginalCollectionChanged);

            innerChange = true;
            foreach (TOriginal x in originalCollection) Add(CreateItem(x));
            innerChange = false;
        }

        private void OriginalCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            innerChange = true;
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewStartingIndex >= 0)
                    {
                        int i = e.NewStartingIndex;
                        foreach (TOriginal item in e.NewItems ?? Array.Empty<TOriginal>())
                        {
                            Insert(i, CreateItem(item));
                            i++;
                        }
                    }
                    else
                    {
                        foreach (TOriginal x in e.NewItems ?? Array.Empty<TOriginal>()) Add(CreateItem(x));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.OldStartingIndex >= 0)
                    {
                        for (int i = 0; i < e.OldItems?.Count; i++) RemoveAtCore(e.OldStartingIndex);
                    }
                    else
                    {
                        foreach (TOriginal x in e.OldItems ?? Array.Empty<TOriginal>()) RemoveCore(x);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (e.NewStartingIndex >= 0)
                    {
                        for (int i = 0; i < e.NewItems?.Count; i++)
                        {
                            this[i + e.NewStartingIndex] = CreateItem((TOriginal)e.NewItems[i]!);
                        }
                    }
                    else
                    {
                        foreach (TOriginal x in e.OldItems ?? Array.Empty<TOriginal>()) RemoveCore(x);
                        foreach (TOriginal x in e.NewItems ?? Array.Empty<TOriginal>()) Add(CreateItem(x));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    for (int i = 0; i < e.NewItems?.Count; i++) Move(e.OldStartingIndex + i, e.NewStartingIndex + i);
                }
                else // Reset
                {
                    ClearCore();
                    foreach (TOriginal x in originalCollection) Add(CreateItem(x));
                }
            }
            finally
            {
                innerChange = false;
            }
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, T item)
        {
            if (!innerChange) throw new NotSupportedException("Insert is not supported.");
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, T item)
        {
            if (!innerChange) throw new NotSupportedException("SetItem is not supported.");
            base.SetItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            if (innerChange) base.RemoveItem(index);
            else ModifyOriginalList().RemoveAt(index);
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            if (innerChange) base.ClearItems();
            else ModifyOriginalList().Clear();
        }

        /// <inheritdoc />
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (innerChange) base.MoveItem(oldIndex, newIndex);
            else ModifyOriginalList().Move(oldIndex, newIndex);
        }

        private ObservableCollection<TOriginal> ModifyOriginalList()
        {
            if (isReadOnly) throw new NotSupportedException("IsReadOnly is true. Modifications are not allowed.");
            return originalList!;
        }

        private T CreateItem([AllowNull] TOriginal oldItem)
        {
            T newItem = factory(oldItem!);
            mapping.Add((oldItem!, newItem));
            return newItem;
        }

        private void RemoveCore([AllowNull] TOriginal oldItem)
        {
            var tuple = mapping.First(t => originalItemComparer.Equals(t.original, oldItem!));
            mapping.Remove(tuple);
            Remove(tuple.newItem);
        }

        private void RemoveAtCore(int index)
        {
            T newItem = this[index];
            var tuple = mapping.First(t => itemComparer.Equals(t.newItem, newItem));
            mapping.Remove(tuple);
            RemoveAt(index);
        }

        private void ClearCore()
        {
            Clear();
            mapping.Clear();
        }
    }
}
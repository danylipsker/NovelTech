using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFLibrary.customAttachedProperties;

namespace WPFLibrary.behaviors
{
    public class BaseDragAndDropContainerItemBehavior<T> : Behavior<ItemsControl> where T : FrameworkElement
    {
        #region Properties

        #region Dependency Property - Effects
        public DragDropEffects Effect
        {
            get { return (DragDropEffects)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }

        public static readonly DependencyProperty EffectProperty =
            DependencyProperty.Register("Effect", typeof(DragDropEffects), typeof(BaseDragAndDropContainerItemBehavior<T>),
                new PropertyMetadata(DragDropEffects.Move));
        #endregion

        #region Dependency Property - IsItemDropTarget
        public bool AllowItemDropTarget
        {
            get { return (bool)GetValue(IsItemDropTargetProperty); }
            set { SetValue(IsItemDropTargetProperty, value); }
        }

        public static readonly DependencyProperty IsItemDropTargetProperty =
            DependencyProperty.Register("IsItemDropTarget", typeof(bool), typeof(BaseDragAndDropContainerItemBehavior<T>), new PropertyMetadata(false));
        #endregion

        #region Dependency Property - IsControlDropTarget
        public bool AllowControlDropTarget
        {
            get { return (bool)GetValue(IsControlDropTargetProperty); }
            set { SetValue(IsControlDropTargetProperty, value); }
        }

        public static readonly DependencyProperty IsControlDropTargetProperty =
            DependencyProperty.Register("IsControlDropTarget", typeof(bool), typeof(BaseDragAndDropContainerItemBehavior<T>), new PropertyMetadata(false));
        #endregion

        #region Commands Properties

        #region Dependency Property - DragEnterCommand
        public RelayCommand DragEnterCommand
        {
            get { return (RelayCommand)GetValue(DragEnterCommandProperty); }
            set { SetValue(DragEnterCommandProperty, value); }
        }

        public static readonly DependencyProperty DragEnterCommandProperty =
            DependencyProperty.Register("DragEnterCommand", typeof(RelayCommand), typeof(BaseDragAndDropContainerItemBehavior<T>),
                new PropertyMetadata(null));
        #endregion

        #region Dependency Property - DragEnterCommand
        public RelayCommand DragLeaveCommand
        {
            get { return (RelayCommand)GetValue(DragLeaveCommandProperty); }
            set { SetValue(DragLeaveCommandProperty, value); }
        }

        public static readonly DependencyProperty DragLeaveCommandProperty =
            DependencyProperty.Register("DragLeaveCommand", typeof(RelayCommand), typeof(BaseDragAndDropContainerItemBehavior<T>),
                new PropertyMetadata(null));
        #endregion

        #region Dependency Property - DropCommand
        public RelayCommand DropCommand
        {
            get { return (RelayCommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register("DropCommand", typeof(RelayCommand), typeof(BaseDragAndDropContainerItemBehavior<T>),
                new PropertyMetadata(null));
        #endregion

        #endregion

        #region Dependency Property - VisualizeOnOver
        public bool VisualizeOnOver
        {
            get { return (bool)GetValue(VisualizeOnOverProperty); }
            set { SetValue(VisualizeOnOverProperty, value); }
        }

        public static readonly DependencyProperty VisualizeOnOverProperty =
            DependencyProperty.Register("VisualizeOnOver", typeof(bool), typeof(BaseDragAndDropContainerItemBehavior<T>), new PropertyMetadata(false));
        #endregion


        #region Dependency Property - DropOnSelf
        public bool DropOnSelf
        {
            get { return (bool)GetValue(DropOnSelfProperty); }
            set { SetValue(DropOnSelfProperty, value); }
        }

        public static readonly DependencyProperty DropOnSelfProperty =
            DependencyProperty.Register("DropOnSelf", typeof(bool), typeof(BaseDragAndDropContainerItemBehavior<T>), new PropertyMetadata(false));
        #endregion


        #endregion
        #region Fields
        private readonly EventSetter _previewMouseLeftButtonEventSetter;
        private readonly EventSetter _previewMouseMoveEventSetter;
        private readonly EventSetter _dragEnterEventSetter;
        private readonly EventSetter _dragLeaveEventSetter;
        private readonly EventSetter _dropEventSetter;
        private readonly Setter _allowDropSetter;
        internal T draggedItem;
        private Control overedItem;
        private System.Windows.Point _mouseStartPoint;
        private System.Windows.Media.Brush originalBorderBrush;
        private Thickness originalBorderThickness;
        public virtual Predicate<T> canDragPredicate { get; set; }
        public virtual Predicate<T> canDropPredicate { get; set; }
        public virtual Func<object> getDataFunc { get; set; }
        #endregion
        #region Constructor
        public BaseDragAndDropContainerItemBehavior()
        {
            _previewMouseLeftButtonEventSetter = new EventSetter(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonDown));
            _previewMouseMoveEventSetter = new EventSetter(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(OnPreviewMouseMove));
            _dragEnterEventSetter = new EventSetter(UIElement.DragEnterEvent, new DragEventHandler(OnDragEnter));
            _dragLeaveEventSetter = new EventSetter(UIElement.DragLeaveEvent, new DragEventHandler(OnDragLeave));
            _dropEventSetter = new EventSetter(UIElement.DropEvent, new DragEventHandler(OnDrop));
            _allowDropSetter = new Setter(UIElement.AllowDropProperty, true);
        }


        #endregion
        #region Methods
        #region Behavior Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AllowControlDropTarget && AssociatedObject is UIElement)
            {
                AssociatedObject.AllowDrop = true;
                AssociatedObject.DragEnter += OnDragEnter;
                AssociatedObject.DragLeave += OnDragLeave;
                AssociatedObject.Drop += OnDrop;
            }
            AssociatedObject.Initialized += (s, e) =>
            {
                UpdateItemContainerStyle();
            };
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AllowControlDropTarget && AssociatedObject is UIElement)
            {
                AssociatedObject.DragEnter -= OnDragEnter;
                AssociatedObject.DragLeave -= OnDragLeave;
                AssociatedObject.Drop -= OnDrop;
            }
            if (AssociatedObject != null)
            {
                AssociatedObject.ItemContainerStyle?.Setters?.Remove(_previewMouseMoveEventSetter);
                AssociatedObject.ItemContainerStyle?.Setters?.Remove(_previewMouseLeftButtonEventSetter);
                if (AllowItemDropTarget)
                {
                    AssociatedObject.ItemContainerStyle.Setters.Remove(_allowDropSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Remove(_dragEnterEventSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Remove(_dragLeaveEventSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Remove(_dropEventSetter);
                }
            }
        }
        private void UpdateItemContainerStyle()
        {
            if (AssociatedObject.ItemContainerStyle == null)
                AssociatedObject.ItemContainerStyle = new Style(typeof(T), Application.Current.TryFindResource(typeof(T)) as Style);

            if (!AssociatedObject.ItemContainerStyle.Setters.Contains(_previewMouseMoveEventSetter))
            {
                AssociatedObject.ItemContainerStyle.Setters.Add(_previewMouseMoveEventSetter);
                AssociatedObject.ItemContainerStyle.Setters.Add(_previewMouseLeftButtonEventSetter);
                if (AllowItemDropTarget)
                {
                    AssociatedObject.ItemContainerStyle.Setters.Add(_allowDropSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Add(_dragEnterEventSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Add(_dragLeaveEventSetter);
                    AssociatedObject.ItemContainerStyle.Setters.Add(_dropEventSetter);
                }
            }

            
        }
        #endregion

        #region Drag And Drop Events
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            _mouseStartPoint = e.GetPosition(null);
        }
        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point currMousePos = e.GetPosition(null);
            Vector diff = _mouseStartPoint - currMousePos;
            if (!draggableAttachedProperty.GetIsBlockDragging(sender as UIElement) && draggedItem == null && e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                if (canDragPredicate == null || canDragPredicate(sender as T) == true)
                {
                    draggedItem = sender as T;
                    object draggedData;
                    if (getDataFunc == null)
                        draggedData = draggedItem.DataContext;
                    else
                        draggedData = getDataFunc();
                    draggedItem.GiveFeedback += (sender, e) =>
                    {
                        if (e.Effects == DragDropEffects.None)
                            draggedItem = null;
                    };
                    DragDrop.DoDragDrop(draggedItem, draggedData, Effect);
                }
                e.Handled = true ;
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (VisualizeOnOver)
            {
                overedItem = sender as Control;
                originalBorderBrush = overedItem.BorderBrush;
                originalBorderThickness = overedItem.BorderThickness;
                overedItem.BorderThickness = new Thickness(1);
                overedItem.BorderBrush = System.Windows.Media.Brushes.Blue;
            }
            var sourceData = e.Data.GetData(e.Data.GetFormats()[0]);
            var targetData = (sender as FrameworkElement).DataContext;
            if (DragEnterCommand != null && (DropOnSelf || sourceData != targetData))
                    DragEnterCommand.Execute(new Tuple<object, object>(sourceData, targetData));
            e.Handled = true;
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if (overedItem != null)
            {
                overedItem.BorderBrush = originalBorderBrush;
                overedItem.BorderThickness = originalBorderThickness;
            }
            var sourceData = e.Data.GetData(e.Data.GetFormats()[0]);
            var targetData = (sender as FrameworkElement).DataContext;
            if (DragLeaveCommand != null && (DropOnSelf || sourceData != targetData))
                DragLeaveCommand.Execute(new Tuple<object, object>(sourceData, targetData));
            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if(overedItem != null)
            {
                overedItem.BorderBrush = originalBorderBrush;
                overedItem.BorderThickness = originalBorderThickness;
            }
            var sourceData = e.Data.GetData(e.Data.GetFormats()[0]);
            var targetData = (sender as FrameworkElement).DataContext;
            if (DropCommand != null && (DropOnSelf || sourceData != targetData) &&
                (canDropPredicate == null || canDropPredicate(sender as T)))
                    DropCommand.Execute(new Tuple<object, object>(sourceData, targetData));
            draggedItem = null;
            e.Handled = true;
        }
        #endregion
        #endregion
    }

    #region Implementations
    public class DraggableListBoxItemBehavior : BaseDragAndDropContainerItemBehavior<ListBoxItem> { }
    public class DraggableTreeViewItemBehavior : BaseDragAndDropContainerItemBehavior<TreeViewItem>
    {
        public DraggableTreeViewItemBehavior()
        {
            canDragPredicate = (o) => (AssociatedObject as TreeView).SelectedItem != null;
            canDropPredicate = (target) => target == null || IsChildOf((AssociatedObject as TreeView).SelectedItem, target.DataContext) == null;
            getDataFunc = () => (AssociatedObject as TreeView).SelectedItem;
        }

        private TreeViewItem IsChildOf(object source, object target)
        {
            if (source == null)
                return null;
            TreeView treeView = AssociatedObject as TreeView;
            List<TreeViewItem> unintendExpands = new List<TreeViewItem>();
            var sourceContainer = ContainerFromItemRecursive(treeView.ItemContainerGenerator, source);
            return ContainerFromItemRecursive(sourceContainer.ItemContainerGenerator, target);

            TreeViewItem ContainerFromItemRecursive(ItemContainerGenerator root, object item)
            {
                var treeViewItem = root.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem != null)
                    return treeViewItem;
                foreach (var subItem in root.Items)
                {
                    treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem;
                    var search = ContainerFromItemRecursive(treeViewItem.ItemContainerGenerator, item);
                    if (search != null)
                        return search;
                }
                return null;
            }
        }
    }
    #endregion
}

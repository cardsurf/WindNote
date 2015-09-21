using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindNote.Gui.Utilities
{
    public static class VisualTreeTraverser
    {

        public static bool IsParent(DependencyObject parentNode, DependencyObject node)
        {
            return parentNode == VisualTreeHelper.GetParent(node);
        }

        public static bool IsAncestor(DependencyObject ancestorNode, DependencyObject node)
        {
            bool isAncestor = false;
            while (node != null)
            {
                node = VisualTreeHelper.GetParent(node);
                if (ancestorNode == node)
                {
                    isAncestor = true;
                    break;
                }
            }

            return isAncestor;
        }

        public static T FindFirstAncestorOfType<T>(DependencyObject node) where T : DependencyObject
        {
            T ancestor = null;
            while (node != null)
            {
                node = VisualTreeHelper.GetParent(node);
                if (node is T)
                {
                    ancestor = (T) node;
                    break;
                }
            }
            return ancestor;
        }

        public static T FindFirstDescendantOfType<T>(DependencyObject node) where T : DependencyObject
        {
            Stack<DependencyObject> nodes = new Stack<DependencyObject>();
            nodes.Push(node);

            while (nodes.Count > 0)
            {
                DependencyObject currentNode = nodes.Pop();
                if (currentNode is T)
                {
                    return (T) currentNode;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(currentNode); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(currentNode, i);
                    nodes.Push(child);
                }
            }

            return null;
        }

        public static FrameworkElement FindFirstDescendantWithName(FrameworkElement node, string name)
        {
            Stack<FrameworkElement> nodes = new Stack<FrameworkElement>();
            nodes.Push(node);

            while (nodes.Count > 0)
            {
                FrameworkElement currentNode = nodes.Pop();
                if (currentNode.Name == name)
                {
                    return currentNode;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(currentNode); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(currentNode, i);
                    if (child is FrameworkElement)
                    {
                        nodes.Push((FrameworkElement)child);
                    }
                }
            }

            return null;
        }


    }
}

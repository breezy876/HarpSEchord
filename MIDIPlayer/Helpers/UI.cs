using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hscm.Helpers
{
    public static class UIHelpers
    {

        public static List<T> GetVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> objects = new List<T>();
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null)
                {
                    T requestedType = child as T;
                    if (requestedType != null)
                        objects.Add(requestedType);
                    objects.AddRange(GetVisualChildren<T>(child));
                }
            }
            return objects;
        }

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static FrameworkElement GetDescendantByType(FrameworkElement element, Type type)
        {
            if (element == null)
            {
                return null;
            }

            if (element.GetType() == type)
            {
                return element;
            }

            FrameworkElement foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                FrameworkElement visual = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        public static Visual GetAnscestorByType(FrameworkElement element, Type type)
        {
            if (element == null)
            {
                return null;
            }

            if (element.GetType() == type)
            {
                return element;
            }

            FrameworkElement foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            FrameworkElement parent = element;

            while(true)
            {
                parent = element.TemplatedParent as FrameworkElement;
                foundElement = GetDescendantByType(parent, type);
                if (foundElement != null || parent == null)
                    break;
            }
            return foundElement;
        }

        public static FrameworkElement GetTemplatedAnscestorByType(FrameworkElement element, Type type)
        {
            if (element == null)
            {
                return null;
            }

            if (element.GetType() == type)
            {
                return element;
            }

            FrameworkElement foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            FrameworkElement parent = element;

            while (true)
            {
                parent = element.TemplatedParent as FrameworkElement;
                foundElement = GetDescendantByType(parent, type);
                if (foundElement != null || parent == null)
                    break;
            }
            return foundElement;
        }
    }
}

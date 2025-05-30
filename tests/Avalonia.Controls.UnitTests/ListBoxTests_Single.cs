using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Avalonia.UnitTests;
using Avalonia.VisualTree;
using Moq;
using Xunit;

namespace Avalonia.Controls.UnitTests
{
    public class ListBoxTests_Single : ScopedTestBase
    {
        MouseTestHelper _mouse = new MouseTestHelper();
        
        [Fact]
        public void Focusing_Item_With_Tab_Should_Not_Select_It()
        {
            var target = new ListBox
            {
                Template = new FuncControlTemplate(CreateListBoxTemplate),
                ItemsSource = new[] { "Foo", "Bar", "Baz " },
            };

            ApplyTemplate(target);

            target.Presenter.Panel.Children[0].RaiseEvent(new GotFocusEventArgs
            {
                NavigationMethod = NavigationMethod.Tab,
            });

            Assert.Equal(-1, target.SelectedIndex);
        }

        [Fact]
        public void Pressing_Space_On_Focused_Item_With_Ctrl_Pressed_Should_Select_It()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);

                target.Presenter.Panel.Children[0].RaiseEvent(new GotFocusEventArgs
                {
                    NavigationMethod = NavigationMethod.Directional,
                    KeyModifiers = KeyModifiers.Control
                });

                target.Presenter.Panel.Children[0].RaiseEvent(new KeyEventArgs
                {
                    RoutedEvent = InputElement.KeyDownEvent,
                    Key = Key.Space,
                    KeyModifiers = KeyModifiers.Control
                });

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Item_Should_Select_It()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);
                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Selected_Item_Should_Not_Deselect_It()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);
                target.SelectedIndex = 0;

                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Item_Should_Select_It_When_SelectionMode_Toggle()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                    SelectionMode = SelectionMode.Single | SelectionMode.Toggle,
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);

                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Selected_Item_Should_Deselect_It_When_SelectionMode_Toggle()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                    SelectionMode = SelectionMode.Toggle,
                };

                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);
                target.SelectedIndex = 0;

                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(-1, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Selected_Item_Should_Not_Deselect_It_When_SelectionMode_ToggleAlwaysSelected()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                    SelectionMode = SelectionMode.Toggle | SelectionMode.AlwaysSelected,
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);
                target.SelectedIndex = 0;

                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Clicking_Another_Item_Should_Select_It_When_SelectionMode_Toggle()
        {
            using (UnitTestApplication.Start())
            {
                var target = new ListBox
                {
                    Template = new FuncControlTemplate(CreateListBoxTemplate),
                    ItemsSource = new[] { "Foo", "Bar", "Baz " },
                    SelectionMode = SelectionMode.Single | SelectionMode.Toggle,
                };
                AvaloniaLocator.CurrentMutable.Bind<PlatformHotkeyConfiguration>().ToConstant(new PlatformHotkeyConfiguration());
                ApplyTemplate(target);
                target.SelectedIndex = 1;

                _mouse.Click(target.Presenter.Panel.Children[0]);

                Assert.Equal(0, target.SelectedIndex);
            }
        }

        [Fact]
        public void Setting_Item_IsSelected_Sets_ListBox_Selection()
        {
            var target = new ListBox
            {
                Template = new FuncControlTemplate(CreateListBoxTemplate),
                ItemsSource = new[] { "Foo", "Bar", "Baz " },
            };

            ApplyTemplate(target);

            ((ListBoxItem)target.GetLogicalChildren().ElementAt(1)).IsSelected = true;

            Assert.Equal("Bar", target.SelectedItem);
            Assert.Equal(1, target.SelectedIndex);
        }

        [Fact]
        public void SelectedItem_Should_Not_Cause_StackOverflow()
        {
            var viewModel = new TestStackOverflowViewModel()
            {
                Items = new List<string> { "foo", "bar", "baz" }
            };

            var target = new ListBox
            {
                Template = new FuncControlTemplate(CreateListBoxTemplate),
                DataContext = viewModel,
                ItemsSource = viewModel.Items
            };

            target.Bind(ListBox.SelectedItemProperty,
                new Binding("SelectedItem") { Mode = BindingMode.TwoWay });

            Assert.Equal(0, viewModel.SetterInvokedCount);

            // In Issue #855, a Stackoverflow occurred here.
            target.SelectedItem = viewModel.Items[2];

            Assert.Equal(viewModel.Items[1], target.SelectedItem);
            Assert.Equal(1, viewModel.SetterInvokedCount);
        }

        private class TestStackOverflowViewModel : INotifyPropertyChanged
        {
            public List<string> Items { get; set; }

            public int SetterInvokedCount { get; private set; }

            public const int MaxInvokedCount = 1000;

            private string _selectedItem;

            public event PropertyChangedEventHandler PropertyChanged;

            public string SelectedItem
            {
                get { return _selectedItem; }
                set
                {
                    if (_selectedItem != value)
                    {
                        SetterInvokedCount++;

                        int index = Items.IndexOf(value);

                        if (MaxInvokedCount > SetterInvokedCount && index > 0)
                        {
                            _selectedItem = Items[index - 1];
                        }
                        else
                        {
                            _selectedItem = value;
                        }

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                    }
                }
            }
        }

        private Control CreateListBoxTemplate(TemplatedControl parent, INameScope scope)
        {
            return new ScrollViewer
            {
                Template = new FuncControlTemplate(CreateScrollViewerTemplate),
                Content = new ItemsPresenter
                {
                    Name = "PART_ItemsPresenter",
                }.RegisterInNameScope(scope)
            };
        }

        private Control CreateScrollViewerTemplate(TemplatedControl parent, INameScope scope)
        {
            return new ScrollContentPresenter
            {
                Name = "PART_ContentPresenter",
                [~ContentPresenter.ContentProperty] =
                    parent.GetObservable(ContentControl.ContentProperty).ToBinding(),
            }.RegisterInNameScope(scope);
        }

        private static void ApplyTemplate(ListBox target)
        {
            // Apply the template to the ListBox itself.
            target.ApplyTemplate();

            // Then to its inner ScrollViewer.
            var scrollViewer = (ScrollViewer)target.GetVisualChildren().Single();
            scrollViewer.ApplyTemplate();

            // Then make the ScrollViewer create its child.
            scrollViewer.Presenter.UpdateChild();

            // Now the ItemsPresenter should be registered, so apply its template.
            target.Presenter.ApplyTemplate();
        }
    }
}

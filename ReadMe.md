# Dynamic Menu Item Data Binding Issue MRE

The WPF application created by this repository tries to dynamically create [`MenuItem`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.menuitem) elements from an [`ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1) using MVVM.

Unfortunately, this doesn't work as expected.

This repository serves as a minimum reproducible example for an issue with dynamic menu item data binding. The MS WPF kindly offered their support in the course of the following discussion items:

- [/dotnet/wpf/discussions/11078](https://github.com/dotnet/wpf/discussions/11078)
- [/dotnet/wpf/discussions/11079](https://github.com/dotnet/wpf/discussions/11079)

The contributors there kindly requested this MRE.

<br/>

## Three Issues Depicted In This MRE

1. The menu items are not updated automatically when the [`FileMruList`](./WpfDataBindingMRE/Code/FileMru/FileMruList.cs) collection changes.

   Consequently, it is currently necessary to **manually** update the menu item data binding using this event handler:

   ```c#
   private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
   {
     FileMenu_MruListSeparatur.GetBindingExpression(VisibilityProperty).UpdateTarget();
     FileMenu_MruList.GetBindingExpression(VisibilityProperty).UpdateTarget();
     BindingOperations.GetBindingExpression
       ( (CollectionViewSource)FileMenu_MruList.FindResource("MruFiles")
       , CollectionViewSource.SourceProperty
       ).UpdateTarget();
   }
   ```

   The above calls should rather not be required. In the MRE code I disabled these by commenting them:

   ```c#
   private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
   {
     //FileMenu_MruListSeparatur.GetBindingExpression(VisibilityProperty).UpdateTarget();
     //FileMenu_MruList.GetBindingExpression(VisibilityProperty).UpdateTarget();
     //BindingOperations.GetBindingExpression
     //  ( (CollectionViewSource)FileMenu_MruList.FindResource("MruFiles")
     //  , CollectionViewSource.SourceProperty
     //  ).UpdateTarget();
   }
   ```

   Running this .NET solution with above lines commented, you will notice that the file MRU list will not be populated despite the fact that, in the `Window_Loaded` event handler, I added two items to the MRU list:

   ```c#
   private void Window_Loaded(object sender, RoutedEventArgs e)
   {
     FileMruList.Add("Test a");
     FileMruList.Add("Test b");
   }
   ```

   <br/>

1. The menu items' item template is composed of an [`AccessText`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.accesstext) element only, as suggested in [/dotnet/wpf/discussions/11078](https://github.com/dotnet/wpf/discussions/11078). Yet, I cannot bind a [`RoutedCommand `](https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.routedcommand) to an `AccessText` element. So, clicking the dynamically created menu items doesn't trigger any command binding.

   <br/>

1. I think I created the menu item data binding by the book:

   ```xml
   <MenuItem.ItemsSource>
     <CompositeCollection>
       <CollectionContainer Collection="{Binding Source={StaticResource ResourceKey=MruFiles}}"/>
       <Separator/>
       <MenuItem x:Name="FileMenu_MruList_Delete" Command="{StaticResource ResourceKey=ClearCmd}"/>
     </CompositeCollection>
   </MenuItem.ItemsSource>
   ```

   However, the `FileMenu_MruList_Delete` menu item generates a XAML binding failure:

   ![XAML Binding Failures](./.doc/XAML%20Binding%20Failures.png)

<br/>

## So, These MRE Questions Kindly Are Awaiting Answers

1. Why doesn't the [`FileMruList`](./WpfDataBindingMRE/Code/FileMru/FileMruList.cs) [`MenuItem`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.menuitem) data binding automatically create the corresponding menu items when the collection is updated?
1. How do I assign a command to the dynamically created menu items for them to eventually work?
1. What's causing the XAML binding failure depicted in the screenshot?

<br/>

## View Model Overview

The main window's view model contains some nested properties; one of them being the `FileMruList` property, which is the property were are dealing with in this MRE. All of the view model classes involved implement the [`INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) interface:

- The main window's view model is a class that implements the [`INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) interface.

- The view model object specifies a public property retrieving another object that itself implements the [`INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) interface.

- That second object specifies a public property retrieving another object derived from[`ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1).

### Here Are Some Important Code Snippets:

##### `MainWindow.xaml`
```xml
<Window x:Class="WpfDataBindingMRE.WpfWindows.Main.MainWindow"
        ...
        DataContext="{Binding RelativeSource={RelativeSource Mode=Self}}"
        >
```

##### `MainWindow.xaml.cs`
```c#
public partial class MainWindow : Window
{
  private readonly FullConfig Settings = new FullConfig();

  private Options Options => Settings.Preferences;
  public FileMruList FileMruList => Options.FileMruList;
```

##### `FullConfig.cs`
```c#
public class FullConfig : _NotifyPropertyChanged
{
  private Options _options;

  public Options Preferences
  {
    get => _options;
    set { _options = value; OnPropertyChanged(); }
  }
```

##### `Options.cs`
```c#
public class Options : _NotifyPropertyChanged, IEquatable<Options>
{
  private FileMruList _fileMruList = null!;

  public FileMruList FileMruList
  {
    get => _fileMruList;
    private init { _fileMruList = value; OnPropertyChanged(); }
  }

```

##### `FileMruList.cs`
```c#
public class FileMruList : ObservableCollection<MruItem>
{
```

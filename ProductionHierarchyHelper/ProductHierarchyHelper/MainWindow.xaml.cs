using PHHTypes;
using ProductionHierarchyHelper.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductHierarchyHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ObservableCollection<Node> nodes = new ObservableCollection<Node>();
		private readonly ObservableCollection<string> allResources = new ObservableCollection<string>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			MakeTestData();
			Load();
			lbNodes.ItemsSource = nodes;
			tabs.Items.Insert(0, CreateNewTab()); //Make sure there is a new tab available
			tabs.SelectedIndex = 0;
			nodes.CollectionChanged += Nodes_CollectionChanged;
		}

		private void MakeTestData()
		{
			Node node = new Node
			{
				Name = "Smelter"
			};
			node.Recipes = new List<Recipe>
			{
				new Recipe
				{
					Name="Iron Ingot",
					ProcessTime = 2,
					Inputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name = "Iron Ore",
								Unit = "Unit"
							},
							1
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW"
							},
							4
						}
					},
					Outputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name="Iron Ingot",
								Unit="Unit"
							},
							1
						}
					}
				},
				new Recipe
				{
					Name="Copper Ingot",
					ProcessTime = 2,
					Inputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name = "Copper Ore",
								Unit = "Unit"
							},
							1
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW"
							},
							4
						}
					},
					Outputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name="Copper Ingot",
								Unit="Unit"
							},
							1
						}
					}
				}
			};
			nodes.Add(node);

			node = new Node
			{
				Name = "Constructor"
			};
			node.Recipes = new List<Recipe>
			{
				new Recipe
				{
					Name="Iron Plate",
					ProcessTime=6,
					Inputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name = "Iron Ingot",
								Unit = "Unit"
							},
							3
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW"
							},
							4
						}
					},
					Outputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name="Iron Plate",
								Unit="Unit"
							},
							2
						}
					}
				},
				new Recipe
				{
					Name="Copper Sheet",
					ProcessTime=6,
					Inputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name = "Copper Ingot",
								Unit = "Unit"
							},
							2
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW"
							},
							4
						}
					},
					Outputs = new Dictionary<Resource, decimal>
					{
						{
							new Resource
							{
								Name="Copper Sheet",
								Unit="Unit"
							},
							1
						}
					}
				}
			};

			nodes.Add(node);

			allResources.Add("Iron Ore");
			allResources.Add("Iron Plate");
			allResources.Add("Copper Ore");
			allResources.Add("Copper Sheet");
			allResources.Add("Energy");
		}

		private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Save();
		}

		private TabItem CreateNewTab()
		{
			TabItem tabItem = new TabItem();
			Binding binding = new Binding("Content.Children[0].Children[1].Text")
			{
				RelativeSource = new RelativeSource(RelativeSourceMode.Self)
			};
			tabItem.SetBinding(TabItem.HeaderProperty, binding);

			Grid outerGrid = new Grid();
			outerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			outerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			outerGrid.RowDefinitions.Add(new RowDefinition());
			outerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Grid innerGrid = new Grid();
			innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			innerGrid.ColumnDefinitions.Add(new ColumnDefinition());
			innerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			innerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			TextBlock lblRecipeName = new TextBlock
			{
				Text = "Recipe Name:",
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			lblRecipeName.SetResourceReference(MarginProperty, "MarginSize");
			innerGrid.Children.Add(lblRecipeName);

			TextBox txtRecipeName = new TextBox
			{
				Name = nameof(txtRecipeName),
				Text = "Recipe"
			};
			txtRecipeName.SetValue(Grid.ColumnProperty, 1);
			innerGrid.Children.Add(txtRecipeName);

			TextBlock lblProcessTime = new TextBlock
			{
				Text = "Process Time:",
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			lblProcessTime.SetResourceReference(MarginProperty, "MarginSize");
			lblProcessTime.SetValue(Grid.RowProperty, 1);
			innerGrid.Children.Add(lblProcessTime);

			TextBox txtProcessTime = new TextBox
			{
				Name = nameof(txtProcessTime)
			};
			txtProcessTime.SetValue(Grid.RowProperty, 1);
			txtProcessTime.SetValue(Grid.ColumnProperty, 1);
			innerGrid.Children.Add(txtProcessTime);

			outerGrid.Children.Add(innerGrid);

			GroupBox gbInputs = new GroupBox
			{
				Name = nameof(gbInputs),
				Header = "Inputs"
			};
			gbInputs.SetValue(Grid.RowProperty, 1);
			Grid grdInputs = new Grid
			{
				Name = nameof(grdInputs)
			};
			grdInputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdInputs.ColumnDefinitions.Add(new ColumnDefinition());
			grdInputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdInputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdInputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Button btnAddInput = new Button
			{
				Name = nameof(btnAddInput),
				Content = "Add new"
			};
			btnAddInput.SetValue(Grid.RowProperty, 9999);
			btnAddInput.Click += btnAddInput_Click;
			grdInputs.Children.Add(btnAddInput);
			gbInputs.Content = grdInputs;
			outerGrid.Children.Add(gbInputs);

			GroupBox gbOutputs = new GroupBox
			{
				Name = nameof(gbOutputs),
				Header = "Outputs"
			};
			gbOutputs.SetValue(Grid.RowProperty, 2);
			Grid grdOutputs = new Grid
			{
				Name = nameof(grdOutputs)
			};
			grdOutputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdOutputs.ColumnDefinitions.Add(new ColumnDefinition());
			grdOutputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdOutputs.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grdOutputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Button btnAddOutput = new Button
			{
				Name = nameof(btnAddOutput),
				Content = "Add new"
			};
			btnAddOutput.SetValue(Grid.RowProperty, 9999);
			btnAddOutput.Click += btnAddOutput_Click;
			grdOutputs.Children.Add(btnAddOutput);
			gbOutputs.Content = grdOutputs;
			outerGrid.Children.Add(gbOutputs);

			Button btnDeleteRecipe = new Button
			{
				Name = nameof(btnDeleteRecipe)
			};
			btnDeleteRecipe.SetValue(Grid.RowProperty, 3);
			btnDeleteRecipe.Click += btnDeleteRecipe_Click;
			outerGrid.Children.Add(btnDeleteRecipe);

			tabItem.Content = outerGrid;

			return tabItem;
		}

		private TabItem PopulateTabItem(TabItem tabItem, Recipe recipe)
		{
			DependencyObject tabContent = tabItem.Content as DependencyObject;
			if (tabContent.FindChild<TextBox>("txtRecipeName") is TextBox txtRecipeName)
				txtRecipeName.Text = recipe.Name;
			if (tabContent.FindChild<TextBox>("txtProcessTime") is TextBox txtProcessTime)
				txtProcessTime.Text = recipe.ProcessTime.ToString("N1");

			if (recipe.Inputs != null && recipe.Inputs.Any() 
				&& tabContent.FindChild<GroupBox>("gbInputs") is GroupBox gbInputs
				&& gbInputs.Content is Grid grdInputs)
			{
				int index = 0;
				foreach (var kvp in recipe.Inputs)
				{
					grdInputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					ComboBox cmbResource = new ComboBox
					{
						Name = nameof(cmbResource),
						ItemsSource = allResources,
						IsEditable = true,
						Text = kvp.Key.Name
					};
					cmbResource.SetValue(Grid.RowProperty, index);

					grdInputs.Children.Add(cmbResource);

					TextBox txtAmount = new TextBox
					{
						Name = nameof(txtAmount),
						Text = kvp.Value.ToString("N0")
					};
					txtAmount.SetValue(Grid.ColumnProperty, 1);
					txtAmount.SetValue(Grid.RowProperty, index);

					grdInputs.Children.Add(txtAmount);

					TextBlock lblUnit = new TextBlock
					{
						Text = kvp.Key.Unit,
						HorizontalAlignment = HorizontalAlignment.Left,
						VerticalAlignment = VerticalAlignment.Center
					};
					lblUnit.SetResourceReference(MarginProperty, "MarginSize");
					lblUnit.SetValue(Grid.ColumnProperty, 2);
					lblUnit.SetValue(Grid.RowProperty, index);

					grdInputs.Children.Add(lblUnit);

					Button btnDeleteInput = new Button
					{
						Content = "Delete"
					};
					btnDeleteInput.Click += btnDeleteInput_Click;
					btnDeleteInput.SetValue(Grid.ColumnProperty, 3);
					btnDeleteInput.SetValue(Grid.RowProperty, index);

					grdInputs.Children.Add(btnDeleteInput);
					index++;
				}
			}
			if (recipe.Outputs != null && recipe.Outputs.Any()
				&& tabContent.FindChild<GroupBox>("gbOutputs") is GroupBox gbOutputs
				&& gbOutputs.Content is Grid grdOutputs)
			{
				int index = 0;
				foreach (var kvp in recipe.Outputs)
				{
					grdOutputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					ComboBox cmbResource = new ComboBox
					{
						Name = nameof(cmbResource),
						ItemsSource = allResources,
						IsEditable = true,
						Text = kvp.Key.Name
					};
					cmbResource.SetValue(Grid.RowProperty, index);

					grdOutputs.Children.Add(cmbResource);

					TextBox txtAmount = new TextBox
					{
						Name = nameof(txtAmount),
						Text = kvp.Value.ToString("N0")
					};
					txtAmount.SetValue(Grid.ColumnProperty, 1);
					txtAmount.SetValue(Grid.RowProperty, index);

					grdOutputs.Children.Add(txtAmount);

					TextBlock lblUnit = new TextBlock
					{
						Text = kvp.Key.Unit,
						HorizontalAlignment = HorizontalAlignment.Left,
						VerticalAlignment = VerticalAlignment.Center
					};
					lblUnit.SetResourceReference(MarginProperty, "MarginSize");
					lblUnit.SetValue(Grid.ColumnProperty, 2);
					lblUnit.SetValue(Grid.RowProperty, index);

					grdOutputs.Children.Add(lblUnit);

					Button btnDeleteOutput = new Button
					{
						Content = "Delete"
					};
					btnDeleteOutput.Click += btnDeleteOutput_Click;
					btnDeleteOutput.SetValue(Grid.ColumnProperty, 3);
					btnDeleteOutput.SetValue(Grid.RowProperty, index);

					grdOutputs.Children.Add(btnDeleteOutput);
					index++;
				}
			}

			return tabItem;
		}

		private void btnDeleteInput_Click(object sender, RoutedEventArgs e)
		{
		}

		private void btnDeleteOutput_Click(object sender, RoutedEventArgs e)
		{
		}

		private void ClearTabs()
		{
			while (tabs.Items.Count > 1)
				tabs.Items.RemoveAt(0);
		}

		private void btnDeleteRecipe_Click(object sender, RoutedEventArgs e)
		{
		}

		private void btnAddInput_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnAddOutput_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			nodes.Add(new Node
			{
				Name = $"Test {nodes.Count}"
			});
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{
			lbNodes.SelectedItem = null;
			ClearTabs();
			tabs.Items.Insert(0, CreateNewTab());
			tabs.SelectedIndex = 0;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (lbNodes.SelectedItems.Count != 1)
			{
				return;
			}

			MessageBoxResult result = MessageBox.Show(this, "Are you sure you want to delete this node?", "Confirm delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
			if (result != MessageBoxResult.OK)
			{
				return;
			}

			nodes.Remove((Node)lbNodes.SelectedItem);
		}

		private void lbNodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lbNodes.SelectedItems.Count == 1)
			{
				btnDelete.IsEnabled = true;
				PopulateNode((Node)lbNodes.SelectedItem);
			}
			else
			{
				btnDelete.IsEnabled = false;
				btnClear_Click(null, null);
			}
		}

		private void PopulateNode(Node node)
		{
			txtName.Text = node.Name;
			ClearTabs();
			if (node.Recipes != null && node.Recipes.Any())
			{
				int index = 0;
				foreach (Recipe r in node.Recipes)
				{
					tabs.Items.Insert(index, PopulateTabItem(CreateNewTab(), r));
					index++;
				}
			}
			else
			{
				tabs.Items.Insert(0, CreateNewTab());
			}
			tabs.SelectedIndex = 0;
		}

		private void Save()
		{

		}

		private void Load()
		{

		}

		private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void btnNew_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

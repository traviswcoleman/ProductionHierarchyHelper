using Microsoft.Win32;
using Newtonsoft.Json;
using PHHTypes;
using ProductionHierarchyHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductionHierarchyHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ObservableCollection<Node> nodes = new ObservableCollection<Node>();
		private readonly ObservableCollection<Resource> allResources = new ObservableCollection<Resource>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Load();
			lbNodes.ItemsSource = nodes;
			tabs.Items.Insert(0, CreateNewTab()); //Make sure there is a new tab available
			tabs.SelectedIndex = 0;
			nodes.CollectionChanged += Nodes_CollectionChanged;
			tabs.SelectionChanged += tabs_SelectionChanged;
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
					Inputs = new List<Resource>
					{
						{
							new Resource
							{
								Name = "Iron Ore",
								Unit = "Unit",
								Amount = 1
							}
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW",
								Amount=4
							}
						}
					},
					Outputs = new List<Resource>
					{
						{
							new Resource
							{
								Name="Iron Ingot",
								Unit="Unit",
								Amount = 1
							}
						}
					}
				},
				new Recipe
				{
					Name="Copper Ingot",
					ProcessTime = 2,
					Inputs = new List<Resource>
					{
						{
							new Resource
							{
								Name = "Copper Ore",
								Unit = "Unit",
								Amount = 1
							}
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW",
								Amount=4
							}
						}
					},
					Outputs = new List<Resource>
					{
						{
							new Resource
							{
								Name="Copper Ingot",
								Unit="Unit",
								Amount = 1
							}
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
					Inputs = new List<Resource>
					{
						{
							new Resource
							{
								Name = "Iron Ingot",
								Unit = "Unit",
								Amount = 3
							}
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW",
								Amount=4
							}
						}
					},
					Outputs = new List<Resource>
					{
						{
							new Resource
							{
								Name="Iron Plate",
								Unit="Unit",
								Amount=2
							}
						}
					}
				},
				new Recipe
				{
					Name="Copper Sheet",
					ProcessTime=6,
					Inputs = new List<Resource>
					{
						{
							new Resource
							{
								Name = "Copper Ingot",
								Unit = "Unit",
								Amount = 2
							}
						},
						{
							new Resource
							{
								Name="Energy",
								Unit="MW",
								Amount=4
							}
						}
					},
					Outputs = new List<Resource>
					{
						{
							new Resource
							{
								Name="Copper Sheet",
								Unit="Unit",
								Amount=1
							}
						}
					}
				}
			};

			nodes.Add(node);

			allResources.Add(new Resource { Name = "Iron Ore", Unit = "Unit" });
			allResources.Add(new Resource { Name = "Iron Plate", Unit = "Unit" });
			allResources.Add(new Resource { Name = "Copper Ore", Unit = "Unit" });
			allResources.Add(new Resource { Name = "Copper Sheet", Unit = "Unit" });
			allResources.Add(new Resource { Name = "Energy", Unit = "MW" });





		}

		private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Save();
			PopulateAllResources();
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
			outerGrid.RowDefinitions.Add(new RowDefinition());
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
			grdInputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Label lblIResource = new Label
			{
				Content = "Resource"
			};

			Label lblIAmount = new Label
			{
				Content = "Amount"
			};
			lblIAmount.SetValue(Grid.ColumnProperty, 1);

			grdInputs.Children.Add(lblIResource);
			grdInputs.Children.Add(lblIAmount);


			Button btnAddInput = new Button
			{
				Name = nameof(btnAddInput),
				Content = "Add new"
			};
			btnAddInput.SetValue(Grid.RowProperty, 9999);
			btnAddInput.Click += btnAddInput_Click;
			grdInputs.Children.Add(btnAddInput);

			ScrollViewer svInputs = new ScrollViewer
			{
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			};

			svInputs.Content = grdInputs;

			gbInputs.Content = svInputs;
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
			grdOutputs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			Label lblOResource = new Label
			{
				Content = "Resource"
			};

			Label lblOAmount = new Label
			{
				Content = "Amount"
			};
			lblOAmount.SetValue(Grid.ColumnProperty, 1);

			grdOutputs.Children.Add(lblOResource);
			grdOutputs.Children.Add(lblOAmount);

			Button btnAddOutput = new Button
			{
				Name = nameof(btnAddOutput),
				Content = "Add new"
			};
			btnAddOutput.SetValue(Grid.RowProperty, 9999);
			btnAddOutput.Click += btnAddOutput_Click;
			grdOutputs.Children.Add(btnAddOutput);
			

			ScrollViewer svOutputs = new ScrollViewer
			{
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			};

			svOutputs.Content = grdOutputs;
			gbOutputs.Content = svOutputs;
			outerGrid.Children.Add(gbOutputs);

			Button btnDeleteRecipe = new Button
			{
				Name = nameof(btnDeleteRecipe),
				Content = "Delete recipe",
				HorizontalAlignment = HorizontalAlignment.Left
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
			{
				txtRecipeName.Text = recipe.Name;
			}

			if (tabContent.FindChild<TextBox>("txtProcessTime") is TextBox txtProcessTime)
			{
				txtProcessTime.Text = recipe.ProcessTime.ToString("N1");
			}

			if (recipe.Inputs != null && recipe.Inputs.Any()
				&& tabContent.FindChild<GroupBox>("gbInputs") is GroupBox gbInputs
				&& gbInputs.Content is ScrollViewer svInputs
				&& svInputs.Content is Grid grdInputs)
			{
				foreach (Resource resource in recipe.Inputs)
				{
					addIOControlsToGrid(grdInputs, resource);
				}
			}
			if (recipe.Outputs != null && recipe.Outputs.Any()
				&& tabContent.FindChild<GroupBox>("gbOutputs") is GroupBox gbOutputs
				&& gbOutputs.Content is ScrollViewer svOutputs
				&& svOutputs.Content is Grid grdOutputs)
			{
				foreach (Resource resource in recipe.Outputs)
				{
					addIOControlsToGrid(grdOutputs, resource);
				}
			}

			return tabItem;
		}

		private void addIOControlsToGrid(Grid grid, Resource resource = null)
		{
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			ComboBox cmbResource = new ComboBox
			{
				Name = nameof(cmbResource),
				ItemsSource = allResources,
				DisplayMemberPath = "Name",
				IsEditable = true,
				Text = resource?.Name ?? ""
			};
			cmbResource.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 2);

			grid.Children.Add(cmbResource);

			TextBox txtAmount = new TextBox
			{
				Name = nameof(txtAmount),
				Text = resource?.Amount.ToString("N0") ?? ""
			};
			txtAmount.SetValue(Grid.ColumnProperty, 1);
			txtAmount.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 2);

			grid.Children.Add(txtAmount);

			
			TextBox txtUnit = new TextBox
			{
				Name = nameof(txtUnit),
				//Text = resource?.Unit ?? "",
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center
			};
			
			txtUnit.SetValue(Grid.ColumnProperty, 2);
			txtUnit.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 2);

			Binding binding = new Binding
			{
				Source = cmbResource,
				Path = new PropertyPath("SelectedItem.Unit"),
				Mode = BindingMode.OneWay
			};
			txtUnit.SetBinding(TextBox.TextProperty, binding);
			grid.Children.Add(txtUnit);

			Button btnDeleteInput = new Button
			{
				Content = "Delete"
			};
			if (grid.Name == "grdInputs")
				btnDeleteInput.Click += btnDeleteInput_Click;
			else
				btnDeleteInput.Click += btnDeleteOutput_Click;
			btnDeleteInput.SetValue(Grid.ColumnProperty, 3);
			btnDeleteInput.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 2);

			grid.Children.Add(btnDeleteInput);
		}

		private void RemoveRowFromGrid(Grid grid, int rowIndex)
		{
			for (int i = grid.Children.Count - 1; i >= 0; i--)
			{
				if (grid.Children[i].GetValue(Grid.RowProperty) is int controlIndex && controlIndex == rowIndex)
				{
					grid.Children.RemoveAt(i);
				}
			}

			for (int i = 0; i < grid.Children.Count; i++)
			{
				if (grid.Children[i].GetValue(Grid.RowProperty) is int controlIndex && controlIndex > rowIndex)
				{
					grid.Children[i].SetValue(Grid.RowProperty, controlIndex - 1);
				}
			}
		}

		private void btnDeleteInput_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btnDelete)
			{
				int? index = btnDelete.GetValue(Grid.RowProperty) as int?;
				if (index.HasValue)
				{
					if (tabs.SelectedItem is TabItem tabItem
						&& tabItem.Content is DependencyObject tabContent
						&& tabContent.FindChild<GroupBox>("gbInputs") is GroupBox gbInputs
						&& gbInputs.Content is ScrollViewer svInputs
						&& svInputs.Content is Grid grdInputs)
					{
						RemoveRowFromGrid(grdInputs, index.Value);
					}
				}
			}
		}

		private void btnDeleteOutput_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btnDelete)
			{
				int? index = btnDelete.GetValue(Grid.RowProperty) as int?;
				if (index.HasValue)
				{
					if (tabs.SelectedItem is TabItem tabItem
						&& tabItem.Content is DependencyObject tabContent
						&& tabContent.FindChild<GroupBox>("gbOutputs") is GroupBox gbOutputs
						&& gbOutputs.Content is ScrollViewer svOutputs
						&& svOutputs.Content is Grid grdOutputs)
					{
						RemoveRowFromGrid(grdOutputs, index.Value);
					}
				}
			}
		}

		private void ClearTabs()
		{
			tabs.SelectionChanged -= tabs_SelectionChanged;
			tabs.Items.Clear();
			tabs.Items.Add(new TabItem
			{
				Header = "+"
			});
			tabs.SelectionChanged += tabs_SelectionChanged;
		}

		private void btnDeleteRecipe_Click(object sender, RoutedEventArgs e)
		{
			tabs.SelectionChanged -= tabs_SelectionChanged;
			tabs.Items.RemoveAt(tabs.SelectedIndex);
			if (tabs.Items.Count == 1)
			{
				tabs.Items.Insert(0, CreateNewTab());
			}
			tabs.SelectedIndex = 0;
			tabs.SelectionChanged += tabs_SelectionChanged;
		}

		private void btnAddInput_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is Button btnAddInput))
			{
				return;
			}

			if (!(btnAddInput.Parent is Grid grid))
			{
				return;
			}

			addIOControlsToGrid(grid);
		}

		private void btnAddOutput_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is Button btnAddInput))
			{
				return;
			}

			if (!(btnAddInput.Parent is Grid grid))
			{
				return;
			}

			addIOControlsToGrid(grid);
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Node node = GetNodeFromForm();
			if (nodes.Select(n => n.Name).Contains(node.Name, StringComparer.InvariantCultureIgnoreCase))
			{
				Node curNode = nodes.Where(n => n.Name.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
				if (curNode != null)
				{
					nodes[nodes.IndexOf(curNode)] = node;
				}
				else
				{
					nodes.Add(node);
				}
			}
			else
			{
				nodes.Add(node);
			}
		}

		private Node GetNodeFromForm()
		{
			Node node = new Node
			{
				Name = txtName.Text,
				Recipes = new List<Recipe>()
			};

			foreach (TabItem tabItem in tabs.Items)
			{
				Recipe recipe = GetRecipeFromTab(tabItem);
				if (recipe != null)
				{
					node.Recipes.Add(recipe);
				}
			}

			return node;
		}

		private Recipe GetRecipeFromTab(TabItem tabItem)
		{
			if (tabItem == null)
			{
				return null;
			}

			if (tabItem.Header as string == "+" || tabItem.Header as string == "Recipe")
			{
				return null;
			}

			if (!(tabItem.Content is Grid outerGrid)
				|| !(outerGrid.FindChild<TextBox>("txtRecipeName") is TextBox txtRecipeName)
				|| !(outerGrid.FindChild<TextBox>("txtProcessTime") is TextBox txtProcessTime)
				|| string.IsNullOrWhiteSpace(txtProcessTime.Text)
				|| !decimal.TryParse(txtProcessTime.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal processTime)
				|| !(outerGrid.FindChild<GroupBox>("gbInputs") is GroupBox gbInputs)
				|| !(gbInputs.Content is ScrollViewer svInputs)
				|| !(svInputs.Content is Grid grdInputs)
				|| !(outerGrid.FindChild<GroupBox>("gbOutputs") is GroupBox gbOutputs)
				|| !(gbOutputs.Content is ScrollViewer svOutputs)
				|| !(svOutputs.Content is Grid grdOutputs)
				)
			{
				return null;
			}

			Recipe recipe = new Recipe
			{
				Name = txtRecipeName.Text,
				ProcessTime = processTime
			};

			recipe.Inputs = GetResourcesFromGrid(grdInputs);
			recipe.Outputs = GetResourcesFromGrid(grdOutputs);

			return recipe;
		}

		private List<Resource> GetResourcesFromGrid(Grid grid)
		{
			List<Resource> resources = new List<Resource>();

			for (int i = 1; i < grid.RowDefinitions.Count - 1; i++)
			{
				bool resourceFound = false;
				bool amountFound = false;
				bool unitFound = false;
				Resource resource = new Resource();
				for (int j = 1; j < grid.Children.Count; j++)
				{
					if (grid.Children[j].GetValue(Grid.RowProperty) is int rowVal
						&& rowVal == i)
					{
						if (grid.Children[j] is ComboBox cmbResource)
						{
							if (string.IsNullOrWhiteSpace(cmbResource.Text))
							{
								break;
							}

							resource.Name = cmbResource.Text;
							resourceFound = true;
						}
						if (grid.Children[j] is TextBox txtAmount && txtAmount.Name == nameof(txtAmount))
						{
							if (string.IsNullOrWhiteSpace(txtAmount.Text) ||
								!int.TryParse(txtAmount.Text, out int amount))
							{
								break;
							}

							resource.Amount = amount;
							amountFound = true;
						}
						if (grid.Children[j] is TextBox txtUnit && txtUnit.Name == nameof(txtUnit))
						{
							if (string.IsNullOrWhiteSpace(txtUnit.Text))
							{
								break;
							}

							resource.Unit = txtUnit.Text;
							unitFound = true;
						}
					}
					if (resourceFound && amountFound && unitFound)
					{
						resources.Add(resource);
						break;
					}
				}
			}

			return resources;
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{
			txtName.Text = String.Empty;
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
			if (string.IsNullOrWhiteSpace(Properties.UserSettings.Default.LastFile))
			{
				SaveFileDialog dialog = new SaveFileDialog
				{
					Filter = "Json Files (*.json)|*.json",
					DefaultExt = "json"
				};
				bool? result = dialog.ShowDialog(this);

				if (!(result.HasValue && result.Value))
				{
					return;
				}

				Properties.UserSettings.Default.LastFile = dialog.FileName;
				Properties.UserSettings.Default.Save();
			}
			if (!string.IsNullOrWhiteSpace(Properties.UserSettings.Default.LastFile))
			{
				JsonSerializer serializer = new JsonSerializer();
				using StreamWriter sw = new StreamWriter(Properties.UserSettings.Default.LastFile);
				using JsonTextWriter tw = new JsonTextWriter(sw);
				serializer.Serialize(tw, nodes);
			}
		}

		private void Load()
		{
			if (!string.IsNullOrWhiteSpace(Properties.UserSettings.Default.LastFile))
			{
				if (!File.Exists(Properties.UserSettings.Default.LastFile))
				{
					Properties.UserSettings.Default.LastFile = null;
					Properties.UserSettings.Default.Save();
					return;
				}
				JsonSerializer serializer = new JsonSerializer();
				using StreamReader sr = new StreamReader(Properties.UserSettings.Default.LastFile);
				using JsonTextReader jr = new JsonTextReader(sr);
				ObservableCollection<Node> loadedNodes = serializer.Deserialize<ObservableCollection<Node>>(jr);
				if (loadedNodes != null)
				{
					nodes.CollectionChanged -= Nodes_CollectionChanged;
					nodes.Clear();

					foreach (Node n in loadedNodes)
					{
						nodes.Add(n);
					}
					nodes.CollectionChanged += Nodes_CollectionChanged;
					PopulateAllResources();
				}
			}
		}

		private void PopulateAllResources()
		{
			allResources.Clear();
			foreach (Resource s in nodes
				.SelectMany(n => n.Recipes)
				.Where(r => r.Inputs != null)
				.SelectMany(r => r.Inputs)
				.Concat(
					nodes.SelectMany(n => n.Recipes)
					.Where(r => r.Outputs != null)
					.SelectMany(r => r.Outputs)
				)
				.Select(r => (r.Name, r.Unit))
				.Distinct()
				.Select(r => new Resource { Name = r.Name, Unit = r.Unit })
				)
			{
				allResources.Add(s);
			}
		}

		private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (tabs.SelectedIndex == tabs.Items.Count - 1)
			{
				tabs.Items.Insert(tabs.Items.Count - 1, CreateNewTab());
				tabs.SelectedIndex--;
			}
		}
	}
}

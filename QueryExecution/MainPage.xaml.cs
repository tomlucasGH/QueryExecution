using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QueryExecution.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Windows;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QueryExecution
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public class DataSourceList
    {
        public string source_name { get; set; }

        public int source_id { get; set; }
    }
    public  class ConfigClass
{
    public static int MyProperty { get; set; }
      public static  List<DataSourceList> datasourceslist { get; set; }
    }
    public class ListItems
    {
        public string tableName { get; set; }

        public string connectionName { get; set; }
        public string color { get; set; }

        public string image { get; set; }

        public string visibility { get; set; }
         
        public int id { get; set; }
    }

    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
            // ListView1.Items.Add("main");

            setListView();
            

        }

        void setListView()
        {
            QueryContext context = new QueryContext();
      //      context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //    var Datasources = context.datasources.ToList();
            //   List <DataSources> dropdownlist = new List<DataSources>();
            List<DataConnections> connections = new List<DataConnections>();
            List<DataSourceList> datasourceslist = new List<DataSourceList>();
            connections = context.dataconnections.ToList();
            foreach (DataConnections connect in connections)
            {
                //   dropdownlist.Add(new DataSources { button_name = "Add", source_name = connect.Name });
                //   ListView1.Items.Add(new DataSources { source_name = connect.Name , source_id = connect.id});
                datasourceslist.Add(new DataSourceList { source_name = connect.Name, source_id = connect.id });
                //  ConfigClass.datasourceslist.Add(new DataSourceList { source_name = connect.Name, source_id = connect.id });
                //  sourcesDropDown.Items.Add(new DataSources { source_name = connect.Name });
            }
            // ListView1.ItemsSource
            CollectionViewSource childCollection = new CollectionViewSource();
            childCollection.Source = ConfigClass.datasourceslist;
           datasourceslist.Sort(delegate (DataSourceList X, DataSourceList Y)
            {
                if (X.source_name == null && Y.source_name == null) return 0;
                else if (X.source_name == null) return -1;
                else if (Y.source_name == null) return -1;
                else return (X.source_name.CompareTo(Y.source_name));
            });
            

            Binding binding = new Binding();
            binding.Source = childCollection;
            //    BindingOperations.SetBinding(ListView1, ListView.ItemsSourceProperty, binding);
            ListView1.ItemsSource = datasourceslist;

        }
        void setList1ListView()
        {
            QueryContext context = new QueryContext();


            if (context.datatables.Count() == 0)
            {
                 context.datatables.Add(new DataTables { DataConnectionId = 3, TableName = "Test" });
                 context.datatables.Add(new DataTables { DataConnectionId = 3, TableName = "Test2" });
                 context.datatables.Add(new DataTables { DataConnectionId = 3, TableName = "Test3" });
                context.datatables.Add(new DataTables { DataConnectionId = 4, TableName = "Test4" }); 
                 context.SaveChanges();
            }
            if (rootPivot.SelectedIndex == 0)
            {
                var results = from connect in context.dataconnections
                              join table in context.datatables on connect.id equals table.DataConnectionId
                              select new { Connection = connect.Name, Table = table.TableName, ID=table.ID};

                var results2 = from connect in context.dataconnections
                               select new { Connection = connect.Name, connectid = connect.id };

                var results3 = from table in context.datatables
                               select new { connectionid = table.DataConnectionId, tablename = table.TableName,
                                 id = table.ID};

                string previousConn = "";
                foreach (var result in results)
                {
                    if (previousConn == result.Connection)
                    {
                        mitemlist.Add(new ListItems
                        {
                            tableName = result.Table,
                            color = "blue",
                            connectionName = result.Connection,
                            image = "/Assets/plus-sign-22.jpg",
                            visibility = "Visible",
                            id = result.ID
                        });
                    }
                    else
                    {
                        mitemlist.Add(new ListItems
                        {
                            tableName = result.Connection,
                            color = "orange",
                            connectionName = result.Connection,
                           image = "",
                            visibility = "Visible",
                            id = result.ID
                        });
                    }
                        previousConn = result.Connection;
        

                    }
                
            }

            // ListView1.ItemsSource
         //  CollectionViewSource childCollection = new CollectionViewSource();
        //    childCollection.Source = mitemlist;// ConfigClass.datasourceslist;
         /*   mitemlist.Sort(delegate(ListItems X, ListItems Y)
            {
                if (X.tableName == null && Y.tableName == null) return 0;
                else if (X.tableName == null) return -1;
                else if (Y.tableName == null) return -1;
                else return (X.tableName.CompareTo(Y.tableName));
            });
            */
     //       Binding binding = new Binding();
      //      binding.Source = childCollection;
         //       BindingOperations.SetBinding(ListView1, ListView.ItemsSourceProperty, binding);
           List1.ItemsSource = mitemlist;

        }


        public class DataSources
        {
            public string button_name { get; set; }
            public string source_name { get; set; }

            public int source_id { get; set; }
        }

        private void ManageDataSources_Click(object sender, RoutedEventArgs e)
        {
            QueryContext context = new QueryContext();
            Button button = e.OriginalSource as Button;
            if (button.Content.ToString() == "Update")
            {
                PivotSources.Visibility = Windows.UI.Xaml.Visibility.Visible;
                rootPivot.SelectedIndex = 1;
                HeaderText.Text = "Update Data Source";
                DataSourceButton.Content = "Update";

                var query_where1 = from result in context.dataconnections
                                   where result.Name.Equals(button.Tag.ToString())
                                   select result;

                foreach (var result in query_where1)
                {
                    sourcesDropDown.SelectedValue = result.DataSourceType;
                    Connstr.Text = result.ConnectionString;
                    ConnectionName.Text = result.Name;
                    source_ID.Text = result.id.ToString();
               }
  
            }

            if (button.Content.ToString() == "Delete")
            {
                delete_listview_item(button.Tag.ToString(),context);
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }




        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<DataSources> items = new List<DataSources>();
            items.Add(new DataSources { button_name = "select", source_name = "temp" });
            items.Add(new DataSources { button_name = "select2", source_name = "temp" });
            //   ListView1.Items.Add(new Button() {Content = "subtract" });
            ListView1.ItemsSource = items;

        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public List<ListItems> mitemlist = new List<ListItems>();

 
        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (rootPivot.SelectedIndex == 0)
            {
                setList1ListView();
             }
            if (rootPivot.SelectedIndex == 1)
            {
                HeaderText.Text = "Add Data Source";
                DataSourceButton.Content = "Add";
                List<datasources> datasourceslist = new List<datasources>();
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:7604/");
                HttpResponseMessage resp = client.GetAsync("/api/values").Result;
                //This method throws an exception if the HTTP response status is an error code.  
                resp.EnsureSuccessStatusCode();
                var stream = resp.Content.ReadAsStreamAsync().Result;
                JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream));
                JsonSerializer se = new JsonSerializer();
                //     var DeserializedObject = se.Deserialize<DataSources>(jsonReader);
                //            IEnumerable<datasources> result = se.Deserialize<datasources>(jsonReader);
                StreamReader readStream;
                readStream = new StreamReader(stream);
                string data = string.Empty;
                data = readStream.ReadToEnd();
                List<datasources> sources = JsonConvert.DeserializeObject<List<datasources>>(data);
                foreach (datasources item in sources)
                {
                    sourcesDropDown.Items.Add(item.type);

                }
                sourcesDropDown.SelectedIndex = 0;
                Connstr.Text = "";
                ConnectionName.Text = "";



            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            QueryContext context = new QueryContext();
            if (DataSourceButton.Content.ToString() != "Update")  //add new
            {
                add_listview_item();
            }
            else //Update
            {
                var query_where1 = from result in context.dataconnections
                                   where result.id.ToString().Equals(source_ID.Text)
                                   select result;
                foreach (var result in query_where1)
                {
                    delete_listview_item(result.Name,context);
                }

                add_listview_item();
            }
        }
        private void listviewclick(object sender, ItemClickEventArgs e)
        {

        }
        private void delete_listview_item(string source_name, QueryContext context)
        {

            var query_where1 = from result in context.dataconnections
                               where result.Name.Equals(source_name)
                               select result;

                foreach (var result in query_where1)
                {
                 for (int i = 0; i < ListView1.Items.Count; i++)
                 {
                     foreach (DataSourceList sources in ListView1.Items)
                     {
                         if (result.Name == sources.source_name)
                         {

                        //     ListView1.Items.Remove(sources);
                             context.dataconnections.Remove(result);

                         }
                     }
                     }
              

                context.SaveChanges();
                setListView();
return;
}

}
private void add_listview_item()
{
QueryContext context = new QueryContext();
DataSources sources = new DataSources();
sources.source_name = ConnectionName.Text;
DataConnections connections = new DataConnections();
connections.DataSourceType = sourcesDropDown.SelectedValue.ToString();
connections.ConnectionString = Connstr.Text;
connections.Name = ConnectionName.Text;

context.dataconnections.Add(connections);

context.SaveChanges();
            //ListView1.Items.Add(sources);
            setListView();
}

        private void Expand_click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

        }
    }

}

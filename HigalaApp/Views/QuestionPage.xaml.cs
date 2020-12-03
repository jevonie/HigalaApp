using HigalaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class QuestionPage : ContentPage
    {

        DataTemplate yesnoDatatemplate;
        DataTemplate yesnoEntryYesDataTemplate;
        DataTemplate yesnoEntryNoDataTemplate;
        DataTemplate entryDataTemplate;
        DataTemplate optionsCheckboxDataTemplate;
        DataTemplate optionsCheckboxEntryDataTemplate;
        public List<QuestionsAnswerOnline> _questionItemsCollections { get; set; }

        public List<string> publicOptions = new List<string>();

        public List<string> publicOptionsTranspo = new List<string>();

        public List<string> selectedItems;
        public ListView listView { get; set; }

        CustomViewModel customerview = new CustomViewModel();
        public List<QuestionsAnswerOnline> QuestionsAnswerOnline
        {
            get
            {
                return _questionItemsCollections;
            }
            set
            {
                _questionItemsCollections = value;
                customerview.OnPropertyChanged();
                Debug.WriteLine("\tPOPERTY CHANGE{0}", "EVENT FIRED");
            }
        }

        public QuestionPage()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "RadioButton_Experimental" });

            //YES NO ENTRY
            Debug.WriteLine("\tTIBONG saging saging saging {0}", "Calling initiate");
            yesnoEntryYesDataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");
                var radion = new RadioButton
                {
                    Text = "Yes",
                    ClassId = "",

                };
                radion.SetBinding(RadioButton.IsCheckedProperty, "question_answer", BindingMode.TwoWay);

                var radion2 = new RadioButton
                {
                    Text = "No",
                    ClassId = "rdYesNoEntry"
                };
                radion2.SetBinding(RadioButton.IsCheckedProperty, "question_answer", BindingMode.TwoWay);
                var entry = new Entry
                {
                    WidthRequest = 100,
                    ClassId = "rdYesNoEntry"
                };
                entry.SetBinding(Entry.TextProperty, "question_text", BindingMode.TwoWay);
                var radiotackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,

                    Children = { radion, radion2 }
                };
                var finalstackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    Children = { radiotackLayout, entry }
                };

                grid.Children.Add(label, 0, 0);
                grid.Children.Add(finalstackLayout, 1, 0);
                return new ViewCell { View = grid };
            });
            //YES NO ENTRY
            yesnoEntryNoDataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");

                var radion = new RadioButton
                {
                    Text = "Yes"
                };
                radion.SetBinding(RadioButton.IsCheckedProperty, "question_answer", BindingMode.TwoWay);
                var radion2 = new RadioButton
                {
                    Text = "No"
                };
                radion2.SetBinding(RadioButton.IsCheckedProperty, "question_answer", BindingMode.TwoWay);
                var entry = new Entry
                {
                    WidthRequest = 100

                };
                entry.SetBinding(Entry.TextProperty, "question_text", BindingMode.TwoWay);
                var radiotackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { radion, radion2 }
                };
                var finalstackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    Children = { radiotackLayout, entry }
                };

                grid.Children.Add(label, 0, 0);
                grid.Children.Add(finalstackLayout, 1, 0);
                return new ViewCell { View = grid };
            });

            //ENTRY
            entryDataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");
                var entry = new Entry
                {
                    WidthRequest = 100
                };
                entry.SetBinding(Entry.TextProperty, "question_text", BindingMode.TwoWay);
                var mainestackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { entry }
                };
                grid.Children.Add(label, 0, 0);
                grid.Children.Add(mainestackLayout, 1, 0);
                return new ViewCell { View = grid };
            });

            //YES NO 
            yesnoDatatemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");
                var radion = new RadioButton
                {
                    Text = "Yes"
                };
                radion.SetBinding(RadioButton.IsCheckedProperty, "question_answer");
                var radion2 = new RadioButton
                {
                    Text = "No"
                };
                radion2.SetBinding(RadioButton.IsCheckedProperty, "question_answer");
                var radiotackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { radion, radion2 }
                };
                grid.Children.Add(label, 0, 0);
                grid.Children.Add(radiotackLayout, 1, 0);
                return new ViewCell { View = grid };
            }
          );
            //Checkbox options
            optionsCheckboxDataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");
                var checkboxStacklaout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                List<string> options = new List<string>{ "HEADACHE", "DRY_COUGH", "DIFFICULT_IN_BREATHING", "FEVER", "SNEEZING" };
                var checkBoxes = new CheckBox[0];
                foreach (string option in options)
                {
                    var chklabel = new Label
                    {
                        Text = option,
                   
                        
                    };
                    var checkBox = new CheckBox();
                    checkBox.ClassId = option;
                    checkBox.CheckedChanged += Checkbox_CheckedChanged;
                     var lineStacklaout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Children = { checkBox, chklabel }
                    };
                    
                    checkboxStacklaout.Children.Add(lineStacklaout);
                }

                grid.Children.Add(label, 0, 0);
                grid.Children.Add(checkboxStacklaout, 1, 0);
                return new ViewCell { View = grid };
            }
          );

            //Checkbox Entry options 
            optionsCheckboxEntryDataTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {

                    ColumnDefinitions =
                        {
                            new ColumnDefinition(),
                            new ColumnDefinition()
                        },
                    Margin = 10
                };
                var label = new Label();
                label.SetBinding(Label.TextProperty, "question");
                var checkboxStacklaout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                List<string> options = new List<string> { "OWNED_VEHICLE", "TAXI", "JEEPNEY", "MULTICAB", "OTHERS" };
                var checkBoxes = new CheckBox[0];
                foreach (string option in options)
                {
                    var chklabel = new Label
                    {
                        Text = option,


                    };
                    var checkBox = new CheckBox();
                    checkBox.ClassId = option;
                    checkBox.CheckedChanged += Checkbox_CheckedChangedTranspo;
                    var lineStacklaout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Children = { checkBox, chklabel }
                    };

                    checkboxStacklaout.Children.Add(lineStacklaout);
                }
                var entry = new Entry
                {
                    WidthRequest = 100
                };
                entry.SetBinding(Entry.TextProperty, "question_text", BindingMode.TwoWay);
                checkboxStacklaout.Children.Add(entry);

                grid.Children.Add(label, 0, 0);
                grid.Children.Add(checkboxStacklaout, 1, 0);
                return new ViewCell { View = grid };
            }
        );

        }

        private void Checkbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var ob = checkbox.ClassId;

            
            if (e.Value == true)
            {
                publicOptions.Add(ob); //get selected checkbox put in list
            }
            else
            {
                publicOptions.Remove(ob); //checkbox is not checked anymore so remove it from the list
            }
        }

        private void Checkbox_CheckedChangedTranspo(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var ob = checkbox.ClassId;


            if (e.Value == true)
            {
                publicOptionsTranspo.Add(ob); //get selected checkbox put in list
            }
            else
            {
                publicOptionsTranspo.Remove(ob); //checkbox is not checked anymore so remove it from the list
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            QuestionsAnswerOnline = BindingContext as List<QuestionsAnswerOnline>;

            listView = new ListView
            {
                ItemsSource = QuestionsAnswerOnline,
                HasUnevenRows = true,
                SelectedItem = new SelectableItemsView {
                    BackgroundColor = Color.White,
                },
               
                ItemTemplate = new PersonDataTemplateSelector
                {
                    YesNoTemplate = yesnoDatatemplate,
                    YesNoEntryYesTemplate = yesnoEntryYesDataTemplate,
                    YesNoEntryNoTemplate = yesnoEntryNoDataTemplate,
                    EntryTemplate = entryDataTemplate,
                    OptionsCheckboxDataTemplate = optionsCheckboxDataTemplate,
                    OptionsCheckboxEntryDataTemplate = optionsCheckboxEntryDataTemplate
                }

            };
            listView.SeparatorColor = Color.Green;
            listView.ItemTapped += itemtapped;
           Content = listView;
        }
        private void itemtapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = (ListView)sender;
            selectedItem.SelectedItem = null;
        }
        private async void OnClickSave(object sender, EventArgs args)
        {
            var action = await DisplayAlert("Submit?", "Are you sure to submit", "Yes", "No");
            if (action)
            {
                foreach (QuestionsAnswerOnline item in QuestionsAnswerOnline)
                {
                    if (item.question_answer == "true")
                    {
                        item.question_answer = "Y";
                    }
                    else
                    {
                        item.question_answer = "N";
                    }

                    if (item.type_answer == 7)
                    {
                        item.question_answer = String.Join(",", publicOptions);
                    }
                    else if (item.type_answer == 8)
                    {
                        item.question_answer = String.Join(",", publicOptionsTranspo); 
                    }

                    await App.Database.SaveQuestionsAnswerAsync(item);
                    Debug.WriteLine("ID:" + item.ID + "| " + item.question_answer + " --> " + item.question_text, "QUESTION ANSWER");
                }

                QuestionFormOnline questionform = await App.Database.GetQuestionsFormByIDAsync(App.FormID);
                var detailPage = new HistoryDetailsPage();
                detailPage.BindingContext = questionform;

                await Navigation.PushModalAsync(detailPage);
                await Navigation.PopAsync();
            }
        }
        private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }



    }
    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate YesNoTemplate { get; set; }
        public DataTemplate YesNoEntryYesTemplate { get; set; }
        public DataTemplate YesNoEntryNoTemplate { get; set; }
        public DataTemplate EntryTemplate { get; set; }
        public DataTemplate OptionsCheckboxDataTemplate { get; set; }
        public DataTemplate OptionsCheckboxEntryDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            if (((QuestionsAnswerOnline)item).type_answer == 1)
            {
                return YesNoTemplate;
            }
            else if (((QuestionsAnswerOnline)item).type_answer == 2)
            {
                return YesNoEntryYesTemplate;
            }
            else if (((QuestionsAnswerOnline)item).type_answer == 3)
            {
                return YesNoEntryNoTemplate;
            }
            else if (((QuestionsAnswerOnline)item).type_answer == 7)
            {
                return OptionsCheckboxDataTemplate;
            }
            else if (((QuestionsAnswerOnline)item).type_answer == 8)
            {
                return OptionsCheckboxEntryDataTemplate;
            }
            else
            {
                return EntryTemplate;
            }
        }
    }

    public class CustomViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;


        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
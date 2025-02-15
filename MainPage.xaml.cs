using CommunityToolkit.Mvvm.Messaging;

namespace MauiGooglePay
{
    public class PayViaGooglePayMessage { }

    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void PayBtn_Clicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send<string>("", "PayViaGooglePay");
            WeakReferenceMessenger.Default.Send(new PayViaGooglePayMessage());
        }


    }

}

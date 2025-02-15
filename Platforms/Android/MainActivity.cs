using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Wallet;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;
using Java.Lang;

namespace MauiGooglePay;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public const int LOAD_PAYMENT_DATA_REQUEST_CODE = 991;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        //MessagingCenter.Subscribe<string>(this, "PayViaGooglePay", (value) => { PayViaGooglePay(); });


        // Subscribe to the message
        WeakReferenceMessenger.Default.Register<PayViaGooglePayMessage>(this, (recipient, message) =>
        {
            PayViaGooglePay();
        });

    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        if (requestCode == LOAD_PAYMENT_DATA_REQUEST_CODE && resultCode == Result.Ok && data is not null)
        {
            PaymentData? paymentData = PaymentData.GetFromIntent(data);
            if (paymentData is not null)
            {
                string paymentInfo = paymentData.ToJson();
                var googleTransactionId = paymentData.GoogleTransactionId;
                var a = paymentData.PaymentMethodToken;
                var b = a.Token;
                //paymentData.PaymentMethodToken.Token.Token
                // Process payment using Pelecard
                //await ProcessPayment(paymentInfo);


                //var response = data.GetStringExtra("extra_api_error_message");
                //var response2 = data.GetStringExtra("reponse");
            }
        }

        base.OnActivityResult(requestCode, resultCode, data);
    }





    void openGooglePay(Activity act, int money)
    {
        PaymentsClient paymentsClient = WalletClass.GetPaymentsClient(
             this,
             new WalletClass.WalletOptions.Builder()
                     //.SetEnvironment(WalletConstants.EnvironmentTest)
                     .SetEnvironment(WalletConstants.EnvironmentTest)
                     .Build()
        );

        TransactionInfo tran = TransactionInfo.NewBuilder()
            .SetTotalPriceStatus(WalletConstants.TotalPriceStatusFinal)
            .SetTotalPrice(money.ToString())
            .SetCurrencyCode("ILS")
            .Build();

        var req = createPaymentDataRequest(tran);

        var futurePay = paymentsClient.LoadPaymentData(req);

        AutoResolveHelper.ResolveTask(futurePay, act, LOAD_PAYMENT_DATA_REQUEST_CODE);
    }

    PaymentDataRequest createPaymentDataRequest(TransactionInfo transactionInfo)
    {
        var paramsBuilder = PaymentMethodTokenizationParameters.NewBuilder()
            .SetPaymentMethodTokenizationType(
            WalletConstants.PaymentMethodTokenizationTypePaymentGateway)
            .AddParameter("gateway", "pelecard")
            .AddParameter("gatewayMerchantId", "123456");

        return createPaymentDataRequest(transactionInfo, paramsBuilder.Build());
    }

    private PaymentDataRequest createPaymentDataRequest(TransactionInfo transactionInfo, PaymentMethodTokenizationParameters paymentMethodTokenizationParameters)
    {
        return PaymentDataRequest.NewBuilder()
            .SetPhoneNumberRequired(false)
            .SetEmailRequired(false)
            .SetShippingAddressRequired(false)
            .SetTransactionInfo(transactionInfo)
            .AddAllowedPaymentMethods(new List<Integer>() { (Integer)WalletConstants.PaymentMethodCard, (Integer)WalletConstants.PaymentMethodTokenizedCard })
            .SetCardRequirements(
                CardRequirements.NewBuilder()
                    .AddAllowedCardNetworks(new List<Integer>() { (Integer)WalletConstants.CardNetworkVisa, (Integer)WalletConstants.CardNetworkMastercard })
                    .SetAllowPrepaidCards(true)
                    .SetBillingAddressFormat(WalletConstants.BillingAddressFormatFull)
                    .Build()
            )
            .SetPaymentMethodTokenizationParameters(paymentMethodTokenizationParameters)
            .SetUiRequired(true)
            .Build();
    }

































    private async void PayViaGooglePay()

    {
        openGooglePay(this, 1);
        //var paymentDataRequest = CreatePaymentDataRequest();
        //var paymentsClient = CreatePaymentsClient(this);

        //var isReadyToPayRequest = IsReadyToPayRequest.NewBuilder()
        //    .AddAllowedPaymentMethod(WalletConstants.PaymentMethodCard)
        //    .Build();

        //bool isReady = await paymentsClient.IsReadyToPay(isReadyToPayRequest).AsAsync();

        //if (isReady)
        //{
        //    // Request payment data
        //    var paymentDataTask = paymentsClient.LoadPaymentData(paymentDataRequest);
        //    StartActivityForResult(paymentDataTask.Intent, RequestCodeGooglePay);
        //}
        //else
        //{
        //    Console.WriteLine("Google Pay is not available on this device.");
        //}
    }

}

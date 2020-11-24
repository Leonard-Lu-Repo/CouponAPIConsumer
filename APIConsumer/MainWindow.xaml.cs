﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;

namespace APIConsumer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private static string url = "https://foxpeer-eval-test.apigee.net/api/coupons";
        private string apiKey="?apikey=ykAAcqrUlfUeYoFro1lTDNuwP1SZwGuT";

    
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_get_Click(object sender, RoutedEventArgs e)
        {
           HttpResponseMessage responseMessage = client.GetAsync(url + apiKey).Result;           
            Coupon[] resultList;
            string jsonResponse = responseMessage.Content.ReadAsStringAsync().Result;
            resultList = JsonConvert.DeserializeObject<Coupon[]>(jsonResponse);
            gridDisplayCoupon.ItemsSource = resultList;
        }

        private void Btn_post_Click(object sender, RoutedEventArgs e)
        {
            PostCoupon postCoupon = new PostCoupon();
            postCoupon.Show();
        }

        private void Btn_put_Click(object sender, RoutedEventArgs e)
        {
            UpdateCoupon updateCoupon = new UpdateCoupon();
            updateCoupon.Show();
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Coupon deleteCoupon = new Coupon();
                deleteCoupon.Id = textCouponID.Text;
                if(textCouponID.Text == "")
                {
                    throw new Exception("Please enter coupon ID!");
                }
                if (MessageBox.Show("Do you want to delete this coupon?", "Warning", MessageBoxButton.OKCancel,
               MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    var deleteResult = client.DeleteAsync(url + "/" + deleteCoupon.Id.ToString() + apiKey).Result;                
                     if (deleteResult.StatusCode == System.Net.HttpStatusCode.NotFound || deleteResult.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
                    {
                        throw new Exception("Please enter correct coupon ID!");
                    }
                   MessageBox.Show(deleteResult.ToString());
                }
                else
                {
                    return;
                }
            }
           catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_getByID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textCouponID.Text == "")
                {
                    throw new Exception("Please enter coupon ID!");
                }
                HttpResponseMessage getByIDResult = client.GetAsync(url + "/" + textCouponID.Text + apiKey).Result;
               if (getByIDResult.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Coupon Not Found, Please enter correct coupon ID!");
                }
                Coupon gettedCouponByID;
               string jsonResponse = getByIDResult.Content.ReadAsStringAsync().Result;
                gettedCouponByID = JsonConvert.DeserializeObject<Coupon>(jsonResponse);

                List<Coupon> getCouponResult = new List<Coupon>();
                getCouponResult.Add(gettedCouponByID);
                gridDisplayCoupon.ItemsSource = getCouponResult;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
          
        }
    }
}

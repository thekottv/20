using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _20.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginCaptcha.xaml
    /// </summary>
    public partial class LoginCaptcha : UserControl
    {
        //public string CaptchaValue { get; set; }
        public bool CaptchaApproved { get; set; }

        public event System.EventHandler CaptchaRefreshed;
        public LoginCaptcha()
        {
            InitializeComponent();

            //CreateCaptcha();
        }
        public void CreateCaptcha()
        {
            CaptchaApproved = false;
            TBX_EnteredCaptcha.Text = string.Empty;

            string allowchar = string.Empty;
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            string[] ar = allowchar.Split(a);
            string pwd = string.Empty;
            string temp = string.Empty;
            System.Random r = new System.Random();

            for (int i = 0; i < 4; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];

                pwd += temp;
            }

            CaptchaText.Text = pwd;

            //CaptchaValue = CaptchaText.Text;

            CaptchaRefreshed?.Invoke(this, System.EventArgs.Empty);
            TBX_EnteredCaptcha.Focus();
        }

        private void BTN_CheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (TBX_EnteredCaptcha.Text == CaptchaText.Text)
            {
                CaptchaApproved = true;
            }
            else
            {
                TBX_EnteredCaptcha.Text = string.Empty;
                CreateCaptcha();
            }
            
        }

        private void BTN_CaptchaRefresh_Click(object sender, RoutedEventArgs e) => CreateCaptcha();
    }
}

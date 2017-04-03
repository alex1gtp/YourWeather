using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using YourWeather.ServiceReference;
namespace YourWeather
{
    public partial class formWeather : Form
    {
        internal Cities.NewDataSet cn;
        public formWeather()
        {
            InitializeComponent();

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 200000000;

            EndpointAddress address = new EndpointAddress("http://www.webservicex.com/globalweather.asmx");

            GlobalWeatherSoapClient gwsc = new GlobalWeatherSoapClient(binding, address);

            var cities = gwsc.GetCitiesByCountry("");

            XmlSerializer result = new XmlSerializer(typeof(Cities.NewDataSet));
            cn = (Cities.NewDataSet)result.Deserialize(new StringReader(cities));

            var Countries = cn.Table.Select(m => m.Country).Distinct();

            comboBoxCountry.Items.AddRange(Countries.ToArray());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rr = cn.Table.Where(m => m.Country == comboBoxCountry.Text).Select(c => c.City);

            comboBoxCities.Items.Clear();
            comboBoxCities.Items.AddRange(rr.ToArray());
        }

        private void comboBoxCities_SelectedIndexChanged(object sender, EventArgs e)
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress("http://www.webservicex.com/globalweather.asmx?WSDL");

            GlobalWeatherSoapClient gwsc = new GlobalWeatherSoapClient(binding, address);

            var weather = gwsc.GetWeather(comboBoxCities.Text, comboBoxCountry.Text);

            if(weather == "Data Not Found!")
            {
                richTextBoxWeatherDetails.Clear();

                //XmlSerializer result = new XmlSerializer(typeof(CurrentWeather));

                //var w = (CurrentWeather)result.Deserialize(new StringReader(weather));

                //for( int i = 0; i< w.ItemsElementName.Lenght; i++)
                //{
                //    richTextBoxWeatherDetails.Text += w.ItmesElementName[i]+ ":" w.Items[i] + "\r\n";
                //}
            }
            else
            {
                richTextBoxWeatherDetails.Clear();
               richTextBoxWeatherDetails.Text = "Data Not Found!";
            }
        }
    }
}

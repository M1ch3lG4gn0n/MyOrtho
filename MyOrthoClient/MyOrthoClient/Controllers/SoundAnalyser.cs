using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOrthoClient.Models;

namespace MyOrthoClient.Controllers
{
    public class SoundAnalyser
    {
        ArrayList x = new ArrayList();
        ArrayList y = new ArrayList();
        ArrayList xin = new ArrayList();
        ArrayList yin = new ArrayList();
        ArrayList vect = new ArrayList();
        ArrayList vectin = new ArrayList();
        
        double X_moy = 0;
        double y_moy = 0;
        double Xin_moy = 0;
        double yin_moy = 0;
        double x_var = 0;
        double y_var = 0;
        double xin_var = 0;
        double yin_var = 0;
        double moyX_Xbar = 0;
        double moyY_Ybar = 0;
        double moyXin_Xinbar = 0;
        double moyYin_Yinbar = 0;
        double[] resultat = { 0.0, 0.0, 0.0, 0.0 };
        double covariance = 0;
        double covariancein = 0;

        double CCC = 0;
        double PCC = 0;
        double CCCin = 0;
        double PCCin = 0;

        public SoundAnalyser()
        {

        }

        public double CalculerMoyenne(ArrayList x)
        {
            double moy = 0;
            for (int i = 0; i < x.Count; i++)
            {
                moy = moy + Convert.ToDouble(x[i]);

            }
            moy = moy / x.Count;
            return moy;

        }

        public double ComputeCoeff(double[] values1, double[] values2)
        {
            /*if (values1.Length != values2.Length)
                throw new ArgumentException("values must be the same length");*/

            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow((x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((y - avg2), 2.0));

            var result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }

        public double ComputeCovariance(IEnumerable<double> source, IEnumerable<double> other)
        {
            int len;
            if (source.Count() < other.Count())
                len = source.Count();
            else
                len = other.Count();

            double avgSource = source.Average();
            double avgOther = other.Average();
            double covariance = 0;

            for (int i = 0; i < len; i++)
                covariance += (source.ElementAt(i) - avgSource) * (other.ElementAt(i) - avgOther);

            return covariance / len;
        }

        public double[] CalculateCorrelation(ICollection<DataLineItem> expected, ICollection<DataLineItem> exercise)
        {
            foreach (var lineItem in expected)
            {   
                x.Add(lineItem.Intensity);
                xin.Add(lineItem.pitch);
            }
            foreach (var lineItem in exercise)
            {
                y.Add(lineItem.Intensity);
                yin.Add(lineItem.pitch);
            }

            double[] Xsample = new double[x.Count];
            double[] Ysample = new double[y.Count];
            double[] Xinsample = new double[xin.Count];
            double[] Yinsample = new double[yin.Count];

            int lenght = 0;

            if (x.Count > y.Count)
                lenght = y.Count;
            else
                lenght = x.Count;



            for (int i = 0; i < lenght; i++)
            {
                Xsample[i] = Convert.ToDouble(x[i]);
                Ysample[i] = Convert.ToDouble(y[i]);
                Xinsample[i] = Convert.ToDouble(xin[i]);
                Yinsample[i] = Convert.ToDouble(yin[i]);
            }

            // calcul de la moyenne
            X_moy = CalculerMoyenne(x);
            Xin_moy = CalculerMoyenne(xin);
            y_moy = CalculerMoyenne(y);
            yin_moy = CalculerMoyenne(yin);

            var corr = ComputeCoeff(Xsample.ToArray(), Ysample.ToArray());
            double cov = ComputeCovariance(Xsample, Ysample);

            Console.Out.WriteLine("correlation est: " + corr + "\n");
            Console.Out.WriteLine("covariance est: " + cov + "\n");

            for (int i = 0; i < x.Count; i++)
            {
                moyX_Xbar += Math.Pow((Convert.ToDouble(x[i]) - X_moy), 2);
                moyXin_Xinbar += Math.Pow((Convert.ToDouble(xin[i]) - Xin_moy), 2);
            }

            // ********calcul de X_var

            x_var = 1.0 / ((x.Count - 1)) * moyX_Xbar;
            xin_var = 1.0 / ((xin.Count - 1)) * moyXin_Xinbar;
            Console.Out.WriteLine(" xin-var: " + xin_var + "\n");

            // calcul de la somme de (y-ymoy)*2
            for (int i = 0; i < y.Count; i++)
            {
                moyY_Ybar += Math.Pow((Convert.ToDouble(y[i]) - y_moy), 2);
                moyYin_Yinbar += Math.Pow((Convert.ToDouble(yin[i]) - yin_moy), 2);
            }

            // calcul de y_var

            y_var = 1.0 / ((y.Count - 1)) * moyY_Ybar;
            yin_var = 1.0 / ((yin.Count - 1)) * moyYin_Yinbar;

            for (int i = 0; i < lenght; i++)
            {
                vect.Add((Convert.ToDouble(x[i]) - X_moy) * (Convert.ToDouble(y[i]) - y_moy));
                vectin.Add(((Convert.ToDouble(xin[i]) - Xin_moy) * ((Convert.ToDouble(yin[i]) - yin_moy))));
            }

            for (int i = 0; i < lenght; i++)
            {

                covariance += ((Convert.ToDouble(x[i]) - X_moy) * (Convert.ToDouble(y[i]) - y_moy));
                covariancein += ((Convert.ToDouble(xin[i]) - Xin_moy) * (Convert.ToDouble(yin[i]) - yin_moy));
            }
            covariance = covariance / (y.Count - 1);
            covariancein = covariancein / (yin.Count - 1);

            //double moy=calculerMoyenne(vect);
            //covariance = ;
            Console.Out.WriteLine("covariance: " + covariance + "\n");
            Console.Out.WriteLine("covariancein: " + covariancein + "\n");

            CCC = (2 * covariance) / (x_var + y_var + (Math.Pow((X_moy - y_moy), 2)));
            PCC = covariance / ((Math.Sqrt(x_var)) * (Math.Sqrt(y_var)));
            CCCin = (2 * covariancein) / (xin_var + yin_var + (Math.Pow((Xin_moy - yin_moy), 2)));
            PCCin = covariancein / ((Math.Sqrt(xin_var)) * (Math.Sqrt(yin_var)));

            //formattage des valeurs pour avoir seulement deux decimal après virgule

            PCC = Convert.ToDouble(PCC.ToString("##.#####"));
            PCCin = Convert.ToDouble(PCCin.ToString("##.#####"));
            CCCin = Convert.ToDouble(CCCin.ToString("##.#####"));
            CCC = Convert.ToDouble(CCC.ToString("##.#####"));
            resultat[0] = CCC;
            resultat[1] = PCC;
            resultat[2] = CCCin;
            resultat[3] = PCCin;


            Console.Out.WriteLine("CCC = " + CCC + "   PCC = " + PCC + " CCCin = " + CCCin + "   PCCin = " + PCCin);
            return resultat;
        }


    }
}
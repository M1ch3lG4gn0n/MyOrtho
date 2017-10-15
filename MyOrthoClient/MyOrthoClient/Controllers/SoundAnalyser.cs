using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Controllers
{
    class SoundAnalyser
    {
       /* ArrayList x = new ArrayList();
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

        public void calculateCorrelation()
        {

            string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");

            while(string line in lines){

            }
        }
        

        BufferedReader in = new BufferedReader(new FileReader(racine+pathCorrelation));
		 String line;
        String[] parts; 
				try {
					while((line = in.readLine()) != null)
					  {
						
						  parts = line.split(" ");
						 
						 x.addElement(Double.parseDouble(parts[1]));  
						 
						 xin.addElement(Double.parseDouble(parts[2])); 
						 
					  }
} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				   
				try {
					in.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
		BufferedReader in1 = new BufferedReader(new FileReader(racine + pathCorrelationRef));
String line1;
String[] parts1; 
		   	  
			try {
				while((line1 = in1.readLine()) != null)
				  {
					
					  parts1 = line1.split(" ");
					  
					 y.addElement(Double.parseDouble(parts1[1]));  
					 yin.addElement(Double.parseDouble(parts1[2])); 
				  }
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
				   
			try {
				in1.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			//création de tableau avec la taille des vecteurs
		     double[] Xsample = new double[x.size()];
double[] Ysample = new double[y.size()];
double[] Xinsample = new double[xin.size()];
double[] Yinsample = new double[yin.size()];
			for(int i=0; i<x.size(); i++){
				Xsample[i]= x.elementAt(i);
				Ysample[i]= y.elementAt(i);
				Xinsample[i]= xin.elementAt(i);
				Yinsample[i]= yin.elementAt(i);
			}
		      
		 // calcul de la moyenne
			X_moy = calculerMoyenne(x);
Xin_moy = calculerMoyenne(xin);
y_moy=calculerMoyenne(y);
yin_moy =  calculerMoyenne(yin);

double corr = new PearsonsCorrelation().correlation(Xsample, Ysample);
double cov = new Covariance().covariance(Xsample, Ysample);
System.out.print("correlation est: "+corr+"\n");
System.out.print("covariance est: "+cov+"\n");
			
		 // calcul de la somme de (x-xmoy)*2
		 
		for(int i=0; i<x.size(); i++){		 
			moyX_Xbar += Math.pow((x.elementAt(i)-X_moy),2);
			moyXin_Xinbar += Math.pow((xin.elementAt(i)-Xin_moy),2);
		}
		
		// ********calcul de X_var
		
		x_var= 1.0/((x.size()-1))* moyX_Xbar;
xin_var= 1.0/((xin.size()-1))* moyXin_Xinbar;
System.out.print(" xin-var: "+ xin_var+"\n");
				
		 // calcul de la somme de (y-ymoy)*2
		
		for(int i=0; i<y.size(); i++){	 
			moyY_Ybar += Math.pow((y.elementAt(i)-y_moy),2);
			moyYin_Yinbar += Math.pow((yin.elementAt(i)-yin_moy),2);
		}
		
		// calcul de y_var
		
		y_var= 1.0/((y.size()-1))* moyY_Ybar;
yin_var= 1.0/((yin.size()-1))* moyYin_Yinbar;
		//double var1= variance(x,X_moy);
		//double var2= variance(y,y_moy);
		 
		// ******calcul de covariance
		//covariance1=calculerMoyenne((x.elementAt(i)-X_moy)*(y.elementAt(i)-y_moy));
		for(int i=0; i<y.size(); i++){
			 
			vect.add((x.elementAt(i)-X_moy)* (y.elementAt(i)-y_moy));
			vectin.add((xin.elementAt(i)-Xin_moy)* (yin.elementAt(i)-yin_moy));
			//System.out.print(vect.elementAt(i)+",");
			
		}
		for(int i=0; i<y.size(); i++){
			 
			covariance+=(x.elementAt(i)-X_moy)* (y.elementAt(i)-y_moy);
			covariancein+=(xin.elementAt(i)-Xin_moy)* (yin.elementAt(i)-yin_moy);
		}
		covariance= covariance/(y.size()-1);
		covariancein= covariancein/(yin.size()-1);
		
		//double moy=calculerMoyenne(vect);
		//covariance = ;
		System.out.print("covariance: "+covariance+"\n");
System.out.print("covariancein: "+covariancein+"\n");
CCC=(2* covariance)/(x_var+y_var+(Math.pow((X_moy-y_moy),2)));
		PCC=covariance/((Math.sqrt(x_var))* (Math.sqrt(y_var)));
		CCCin=(2* covariancein)/(xin_var+yin_var+(Math.pow((Xin_moy-yin_moy),2)));
		PCCin=covariancein/((Math.sqrt(xin_var))* (Math.sqrt(yin_var)));
		
		//formattage des valeurs pour avoir seulement deux decimal après virgule
		
		PCC =Double.parseDouble(new DecimalFormat("##.##").format(PCC));
		PCCin =Double.parseDouble(new DecimalFormat("##.##").format(PCCin));
		CCCin =Double.parseDouble(new DecimalFormat("##.##").format(CCCin));
		CCC =Double.parseDouble(new DecimalFormat("##.##").format(CCC));
		resultat[0]=CCC;
		resultat[1]=PCC;
		resultat[2]=CCCin;
		resultat[3]=PCCin;
		
		
		System.out.printf("CCC = "+CCC+"   PCC = "+PCC + " CCCin = "+CCCin+"   PCCin = "+PCCin);
		return resultat;
		 
	 }*/
    }
}

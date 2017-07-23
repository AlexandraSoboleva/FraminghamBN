using System;
using System.IO;
using System.Collections.Generic;
using MicrosoftResearch.Infer.Maths;

namespace FraminghamnBN
{
   public enum modes
    {
        angina=1,
        mi_fdch,
        hospmi,
        anychd,
        stroke,
        cvd,
        hyperten,
        death
    };
    public class FraminghamBN
    {
        #region defineDataSet
        
        static int[] sex;
        static int[] age;
        static int[] sysbp;
        static int[] diabp;
        static int[] bpmeds;
        static int[] cursmoke;
        static int[] totchol;
        static int[] hdlc;
        static int[] ldlc;
        static int[] bmi;
        static int[] glucose;
        static int[] diabetes;
        static int[] prevap;
        static int[] prevchd;
        static int[] prevmi;
        static int[] prevstrk;
        static int[] prevhyp;
        static int[] angina;
        static int[] hospmi;
        static int[] mi_fchd;
        static int[] anychd;
        static int[] stroke;
        static int[] cvd;
        static int[] hyperten;
        static int[] death;

        #endregion

        public static void setData(string path, char delimeter, int count)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string curstr = reader.ReadLine();
                for (int i = 0; i < count; i++)
                {
                    int temp;
                    double dtemp;
                    curstr = reader.ReadLine();
                    string[] curArr = curstr.Split(delimeter);
                    sex[i] = Int32.Parse(curArr[1])-1;

                    //totchol, hdlc, ldlc, bmi - if value is unknown put the most frequent
                    if (curArr[2] == "") totchol[i] = 0;
                    else
                    {
                        temp = Int32.Parse(curArr[3]);
                        if (temp <= 303) totchol[i] = 0;
                        else if (temp <= 499) totchol[i] = 1;
                        else totchol[i] = 2;
                    }

                     temp=Int32.Parse(curArr[3]);
                     if (temp <= 41) age[i] = 0;
                     else if (temp <= 51) age[i] = 1;
                     else if (temp <= 61) age[i] = 2;
                     else if (temp <= 71) age[i] = 3;
                     else age[i] = 4;

                     dtemp = Convert.ToDouble(curArr[4].Replace(".", ","));
                     if (dtemp <= 119) sysbp[i] = 0;
                     else if (dtemp <= 154) sysbp[i] = 1;
                     else if (dtemp <= 189) sysbp[i] = 2;
                     else if (dtemp <= 224) sysbp[i] = 3;
                     else sysbp[i] = 4;
                    
                     dtemp = Convert.ToDouble(curArr[5].Replace(".",","));
                     if (dtemp <= 50) diabp[i] = 0;
                     else if (dtemp <= 70) diabp[i] = 1;
                     else if (dtemp <= 90) diabp[i] = 2;
                     else if (dtemp <= 110) diabp[i] = 3;
                     else if (dtemp <= 130) diabp[i] = 4;
                     else diabp[i] = 5;

                     cursmoke[i] = Int32.Parse(curArr[6]);

                     if (curArr[8] == "") bmi[i] = 2;
                     else 
                    {
                         dtemp = Convert.ToDouble(curArr[8].Replace(".",","));
                         if (dtemp <= 20.48) bmi[i] = 0;
                         else if (dtemp <= 26.53) bmi[i] = 1;
                         else if (dtemp <= 32.58) bmi[i] = 2;
                         else if (dtemp <= 38.63) bmi[i] = 3;
                         else if (dtemp <= 44.68) bmi[i] = 4;
                         else if (dtemp <= 50.73) bmi[i] = 5;
                         else bmi[i] = 6;
                     }
                     
                    diabetes[i] = Int32.Parse(curArr[9]);
                    //if bpmeds is unknown, we suppose that bpmeds not currently used
                    if (curArr[10] == "") bpmeds[i] = 0;else bpmeds[i] = Int32.Parse(curArr[10]);

                    //if patient has diabete, the glucose level is upper
                    if (curArr[12] == "") glucose[i] = diabetes[i];
                    else
                    {
                        temp = Int32.Parse(curArr[12]);
                        if (temp <= 259) glucose[i] = 0;
                        else totchol[i] = 1;
                    }

                    prevchd[i] = Int32.Parse(curArr[14]);
                    prevap[i] = Int32.Parse(curArr[15]);
                    prevmi[i] = Int32.Parse(curArr[16]);
                    prevstrk[i] = Int32.Parse(curArr[17]);
                    prevhyp[i] = Int32.Parse(curArr[18]);

                    if (curArr[21] == "") hdlc[i] = 0;
                    else
                    {
                        temp = Int32.Parse(curArr[21]);
                        if (temp <= 97) hdlc[i] = 0;
                        else if (temp <= 129) hdlc[i] = 1;
                        else hdlc[i] = 2;
                    }

                    if (curArr[22] == "") ldlc[i] = 0;
                    else
                    {
                        temp = Int32.Parse(curArr[22]);
                        if (temp <= 201) ldlc[i] = 0;
                        else if (temp <= 383) ldlc[i] = 1;
                        else ldlc[i] = 2;
                    }

                    death[i] = Int32.Parse(curArr[23]);
                    angina[i] = Int32.Parse(curArr[24]);
                    hospmi[i] = Int32.Parse(curArr[25]);
                    mi_fchd[i] = Int32.Parse(curArr[26]);
                    anychd[i] = Int32.Parse(curArr[27]);
                    stroke[i] = Int32.Parse(curArr[28]);
                    cvd[i] = Int32.Parse(curArr[29]);
                    hyperten[i] = Int32.Parse(curArr[30]);

                }
            }
                
        }
        public static void run()
        {
            
            // Set random seed for repeatable example
            Rand.Restart(12347);

            // Create a new model
            FraminghamModel model = new FraminghamModel();

            /*// Query the model when we know the parameters exactly*/
            //Define parameters
            int datasetCount = 7000;
            #region initDataSet
            sex = new int[datasetCount];
            age = new int[datasetCount];
            sysbp = new int[datasetCount];
            diabp = new int[datasetCount];
            bpmeds = new int[datasetCount];
            cursmoke = new int[datasetCount];
            totchol = new int[datasetCount];
            hdlc = new int[datasetCount];
            ldlc = new int[datasetCount];
            bmi = new int[datasetCount];
            glucose = new int[datasetCount];
            diabetes = new int[datasetCount];
            prevap = new int[datasetCount];
            prevchd = new int[datasetCount];
            prevmi = new int[datasetCount];
            prevstrk = new int[datasetCount];
            prevhyp = new int[datasetCount];
            angina = new int[datasetCount];
            hospmi = new int[datasetCount];
            mi_fchd = new int[datasetCount];
            anychd = new int[datasetCount];
            stroke = new int[datasetCount];
            cvd = new int[datasetCount];
            hyperten = new int[datasetCount];
            death = new int[datasetCount];

            #endregion

            string path = @"FRAMINGHAM_csv\frmgham7K.csv";
            setData(path, ',', datasetCount);
            // Learn posterior distributions for the parameters
            model.LearnParameters(datasetCount,sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, glucose, diabetes,
                                  prevap, prevchd, prevmi, prevstrk, prevhyp, angina, hospmi, mi_fchd, anychd, stroke,
                                  cvd, hyperten, death);

            //Prediction
            datasetCount=1;
            #region initDataSet
            sex = new int[datasetCount];
            age = new int[datasetCount];
            sysbp = new int[datasetCount];
            diabp = new int[datasetCount];
            bpmeds = new int[datasetCount];
            cursmoke = new int[datasetCount];
            totchol = new int[datasetCount];
            hdlc = new int[datasetCount];
            ldlc = new int[datasetCount];
            bmi = new int[datasetCount];
            glucose = new int[datasetCount];
            diabetes = new int[datasetCount];
            prevap = new int[datasetCount];
            prevchd = new int[datasetCount];
            prevmi = new int[datasetCount];
            prevstrk = new int[datasetCount];
            prevhyp = new int[datasetCount];
            angina = new int[datasetCount];
            hospmi = new int[datasetCount];
            mi_fchd = new int[datasetCount];
            anychd = new int[datasetCount];
            stroke = new int[datasetCount];
            cvd = new int[datasetCount];
            hyperten = new int[datasetCount];
            death = new int[datasetCount];

            #endregion
            path = @"FRAMINGHAM_csv\frmgham1K.csv";
            setData(path, ',', 1);
            double probDeathTestSet = model.ProbDisease((int)modes.death,
                sex[0], age[0], sysbp[0], diabp[0], bpmeds[0], cursmoke[0], totchol[0], hdlc[0], ldlc[0], bmi[0], glucose[0],
                diabetes[0], prevap[0], prevchd[0], prevmi[0], prevstrk[0], prevhyp[0], angina[0], hospmi[0], mi_fchd[0],
                anychd[0], stroke[0], cvd[0], hyperten[0], death[0],
                model.ProbSexPosterior, model.ProbBPMedsPosterior, model.ProbPrevAPPosterior, model.ProbPrevCHDPosterior,
                model.ProbPrevMIPosterior, model.ProbPrevStrkPosterior, model.ProbPrevHypPosterior,
                model.CPTAgePosterior, model.CPTHDLCPosterior, model.CPTGlucosePosterior, model.CPTDiabetesPosterior,
                model.CPTBMIPosterior, model.CPTCurSmokePosterior, model.CPTDiaBPPosterior, model.CPTCVDPosterior,
                model.CPTHypertenPosterior, model.CPTSysBPPosterior, model.CPTTotCholPosterior, model.CPTLDLCPosterior,
                model.CPTStrokePosterior, model.CPTDeathPosterior, model.CPTAnginaPosterior, model.CPTAnyCHDPosterior,
                model.CPTMI_FCHDPosterior, model.CPTHospMIPosterior);
           Console.WriteLine("Testing: "+probDeathTestSet+" "+death[0]);
            //Accuracy
           datasetCount = 3000;
           #region initDataSet
           sex = new int[datasetCount];
           age = new int[datasetCount];
           sysbp = new int[datasetCount];
           diabp = new int[datasetCount];
           bpmeds = new int[datasetCount];
           cursmoke = new int[datasetCount];
           totchol = new int[datasetCount];
           hdlc = new int[datasetCount];
           ldlc = new int[datasetCount];
           bmi = new int[datasetCount];
           glucose = new int[datasetCount];
           diabetes = new int[datasetCount];
           prevap = new int[datasetCount];
           prevchd = new int[datasetCount];
           prevmi = new int[datasetCount];
           prevstrk = new int[datasetCount];
           prevhyp = new int[datasetCount];
           angina = new int[datasetCount];
           hospmi = new int[datasetCount];
           mi_fchd = new int[datasetCount];
           anychd = new int[datasetCount];
           stroke = new int[datasetCount];
           cvd = new int[datasetCount];
           hyperten = new int[datasetCount];
           death = new int[datasetCount];

           #endregion
           path = @"FRAMINGHAM_csv\frmgham3K.csv";
           setData(path, ',', datasetCount);

            int sumright=0;
            for (int i = 0; i < datasetCount; i++)
            {
                int res;
                int mode=(int)modes.death;
                probDeathTestSet = model.ProbDisease(mode,
                sex[i], age[i], sysbp[i], diabp[i], bpmeds[i], cursmoke[i], totchol[i], hdlc[i], ldlc[i], bmi[i], glucose[i],
                diabetes[i], prevap[i], prevchd[i], prevmi[i], prevstrk[i], prevhyp[i], angina[i], hospmi[i], mi_fchd[i],
                anychd[i], stroke[i], cvd[i], hyperten[i], death[i],
                model.ProbSexPosterior, model.ProbBPMedsPosterior, model.ProbPrevAPPosterior, model.ProbPrevCHDPosterior,
                model.ProbPrevMIPosterior, model.ProbPrevStrkPosterior, model.ProbPrevHypPosterior,
                model.CPTAgePosterior, model.CPTHDLCPosterior, model.CPTGlucosePosterior, model.CPTDiabetesPosterior,
                model.CPTBMIPosterior, model.CPTCurSmokePosterior, model.CPTDiaBPPosterior, model.CPTCVDPosterior,
                model.CPTHypertenPosterior, model.CPTSysBPPosterior, model.CPTTotCholPosterior, model.CPTLDLCPosterior,
                model.CPTStrokePosterior, model.CPTDeathPosterior, model.CPTAnginaPosterior, model.CPTAnyCHDPosterior,
                model.CPTMI_FCHDPosterior, model.CPTHospMIPosterior);

               if (probDeathTestSet >= 0.5) res = 0; else res = 1;
               switch (mode)
               {
                   case 1:
                       if (res == angina[i]) sumright++;
                       break;
                   case 2:
                       if (res == mi_fchd[i]) sumright++;
                       break;
                   case 3:
                       if (res == hospmi[i]) sumright++;
                       break;
                   case 4:
                       if (res == anychd[i]) sumright++;
                       break;
                   case 5:
                       if (res == stroke[i]) sumright++;
                       break;
                   case 6:
                       if (res == cvd[i]) sumright++;
                       break;
                   case 7:
                       if (res == hyperten[i]) sumright++;
                       break;
                   case 8:
                       if (res == death[i])
                       {
                           sumright++;
                       }
                       break;
               }            
                
            }
            Console.WriteLine("Accurancy: "+(double)sumright / datasetCount);
        }
    }
}

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

        static int[] randid;
        static int[] period;
        static int[] sex;
        static int[] age;
        static int[] time;
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
        static int[] timeap;
        static int[] timemi;
        static int[] timemifc;
        static int[] timechd;
        static int[] timecvd;
        static int[] timestrk;
        static int[] timehyp;
        static int[] death;
        static int[] timedth;

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
                    randid[i] = Int32.Parse(curArr[0])-2448;
                    sex[i] = Int32.Parse(curArr[1])-1;

                    //totchol, hdlc, ldlc, bmi - if value is unknown put the most frequent
                    if (curArr[2] == "") totchol[i] = 0;
                    else
                    {
                        temp = Int32.Parse(curArr[2]);
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
                    
                    temp = Int32.Parse(curArr[19])-1;
                    if (temp <= 1618) time[i] = 0;
                    else if (temp <= 3236) time[i] = 1;
                    else time[i] = 2;

                    period[i]=Int32.Parse(curArr[20])-1;
                    
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

                    temp = Int32.Parse(curArr[31]);
                    if (temp <= 1618) timeap[i] = 0;
                    else if (temp <= 3236) timeap[i] = 1;
                    else if (temp <= 4854) timeap[i] = 2;
                    else if (temp <= 6472) timeap[i] = 3;
                    else if (temp <= 8090) timeap[i] = 4;
                    else timeap[i] = 5;
                    temp = Int32.Parse(curArr[32]);
                    if (temp <= 1618) timemi[i] = 0;
                    else if (temp <= 3236) timemi[i] = 1;
                    else if (temp <= 4854) timemi[i] = 2;
                    else if (temp <= 6472) timemi[i] = 3;
                    else if (temp <= 8090) timemi[i] = 4;
                    else timemi[i] = 5;
                    temp = Int32.Parse(curArr[33]);
                    if (temp <= 1618) timemifc[i] = 0;
                    else if (temp <= 3236) timemifc[i] = 1;
                    else if (temp <= 4854) timemifc[i] = 2;
                    else if (temp <= 6472) timemifc[i] = 3;
                    else if (temp <= 8090) timemifc[i] = 4;
                    else timemifc[i] = 5;
                    temp = Int32.Parse(curArr[34]);
                    if (temp <= 1618) timechd[i] = 0;
                    else if (temp <= 3236) timechd[i] = 1;
                    else if (temp <= 4854) timechd[i] = 2;
                    else if (temp <= 6472) timechd[i] = 3;
                    else if (temp <= 8090) timechd[i] = 4;
                    else timechd[i] = 5;
                    temp = Int32.Parse(curArr[35]);
                    if (temp <= 1618) timestrk[i] = 0;
                    else if (temp <= 3236) timestrk[i] = 1;
                    else if (temp <= 4854) timestrk[i] = 2;
                    else if (temp <= 6472) timestrk[i] = 3;
                    else if (temp <= 8090) timestrk[i] = 4;
                    else timestrk[i] = 5;
                    temp = Int32.Parse(curArr[36]);
                    if (temp <= 1618) timecvd[i] = 0;
                    else if (temp <= 3236) timecvd[i] = 1;
                    else if (temp <= 4854) timecvd[i] = 2;
                    else if (temp <= 6472) timecvd[i] = 3;
                    else if (temp <= 8090) timecvd[i] = 4;
                    else timecvd[i] = 5;
                    temp = Int32.Parse(curArr[37]);
                    if (temp <= 1618) timedth[i] = 0;
                    else if (temp <= 3236) timedth[i] = 1;
                    else if (temp <= 4854) timedth[i] = 2;
                    else if (temp <= 6472) timedth[i] = 3;
                    else if (temp <= 8090) timedth[i] = 4;
                    else timedth[i] = 5;
                    timehyp[i] = Int32.Parse(curArr[38]);
                    if (temp <= 1618) timehyp[i] = 0;
                    else if (temp <= 3236) timehyp[i] = 1;
                    else if (temp <= 4854) timehyp[i] = 2;
                    else if (temp <= 6472) timehyp[i] = 3;
                    else if (temp <= 8090) timehyp[i] = 4;
                    else timehyp[i] = 5;
                }
            }
                
        }
        public static void run()
        {
            
            // Set random seed for repeatable example
            Rand.Restart(12347);

            // Create a new model
            FraminghamModel model = new FraminghamModel();

            // Query the model when we know the parameters exactly
            //Define parameters
            int datasetCount = 6000;
            #region initDataSet
            randid = new int[datasetCount];
            period = new int[datasetCount];
            sex = new int[datasetCount];
            age = new int[datasetCount];
            time = new int[datasetCount];
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
            timemi = new int[datasetCount];
            timemifc = new int[datasetCount];
            timeap = new int[datasetCount];
            timechd = new int[datasetCount];
            timecvd = new int[datasetCount];
            timestrk = new int[datasetCount];
            timehyp = new int[datasetCount];
            death = new int[datasetCount];
            timedth = new int[datasetCount];

            #endregion

            string path = @"D:\9-работа\helix\data\FRAMINGHAM_csv\frmgham6K.csv";
            setData(path, ',', datasetCount);
            // Learn posterior distributions for the parameters
           model.LearnParameters(datasetCount, sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, 
                                 glucose, diabetes, prevap, prevchd, prevmi, prevstrk, prevhyp, angina, hospmi, 
                                 mi_fchd, anychd, stroke, cvd, hyperten, death, randid, period, time, timeap, timemi,
                                 timemifc, timechd, timestrk, timecvd, timehyp, timedth);
            
            //Prediction
            datasetCount=1;
            #region initDataSet
            randid = new int[datasetCount];
            period = new int[datasetCount];
            sex = new int[datasetCount];
            age = new int[datasetCount];
            time = new int[datasetCount];
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
            timemi = new int[datasetCount];
            timemifc = new int[datasetCount];
            timeap = new int[datasetCount];
            timechd = new int[datasetCount];
            timecvd = new int[datasetCount];
            timestrk = new int[datasetCount];
            timehyp = new int[datasetCount];
            death = new int[datasetCount];
            timedth = new int[datasetCount];

            #endregion
            path = @"D:\9-работа\helix\data\FRAMINGHAM_csv\frmgham6K.csv";
            setData(path, ',', 1);
            double probDeathTestSet = model.ProbDisease(false,(int)modes.anychd,
                sex[0], age[0], sysbp[0], diabp[0], bpmeds[0], cursmoke[0], totchol[0], hdlc[0], ldlc[0], bmi[0], glucose[0],
                diabetes[0], prevap[0], prevchd[0], prevmi[0], prevstrk[0], prevhyp[0], angina[0], hospmi[0], mi_fchd[0],
                anychd[0], stroke[0], cvd[0], hyperten[0], death[0], randid[0], period[0], time[0],
                5,5,5,5,5,5,5,5,
                model.ProbSexPosterior, model.ProbBPMedsPosterior, model.CPTPrevAPPosterior, model.CPTPrevCHDPosterior,
                model.CPTPrevMIPosterior, model.CPTPrevStrkPosterior, model.CPTPrevHypPosterior,
                model.CPTAgePosterior, model.CPTHDLCPosterior, model.CPTGlucosePosterior, model.CPTDiabetesPosterior,
                model.CPTBMIPosterior, model.CPTCurSmokePosterior, model.CPTDiaBPPosterior, model.CPTCVDPosterior,
                model.CPTHypertenPosterior, model.CPTSysBPPosterior, model.CPTTotCholPosterior, model.CPTLDLCPosterior,
                model.CPTStrokePosterior, model.CPTDeathPosterior, model.CPTAnginaPosterior, model.CPTAnyCHDPosterior,
                model.CPTMI_FCHDPosterior, model.CPTHospMIPosterior,
                null, model.ProbPerioddPosterior, model.CPTTimePosterior, model.CPTTimeAPPosterior, model.CPTTimeMIPosterior,
                model.CPTTimeMIFCPosterior, model.CPTTimeCHDPosterior, model.CPTTimeCVDPosterior, model.CPTTimeSTRKPosterior, model.CPTTimeHypPosterior, model.CPTTimeDthPosterior
                );
            Console.WriteLine("Testing: " + probDeathTestSet + " " + prevchd[0]+" time: "+timechd[0]);
            using (StreamReader reader = new StreamReader(path))
            {
                string curstr = reader.ReadLine();
                curstr = reader.ReadLine();
                int riskperr = FraminghamRiskScore.risk(curstr, ',');
                Console.WriteLine("Framingham Risk Score: " + riskperr+ " %");
            }

            //BN vs Framingham Risk Score
            datasetCount = 627;
            #region initDataSet
            randid = new int[datasetCount];
            period = new int[datasetCount];
            sex = new int[datasetCount];
            age = new int[datasetCount];
            time = new int[datasetCount];
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
            timeap = new int[datasetCount];
            timemi = new int[datasetCount];
            timemifc = new int[datasetCount];
            timechd = new int[datasetCount];
            timecvd = new int[datasetCount];
            timehyp = new int[datasetCount];
            timestrk = new int[datasetCount];
            death = new int[datasetCount];
            timedth = new int[datasetCount];

            #endregion
            path = @"D:\9-работа\helix\data\FRAMINGHAM_csv\frmgham627.csv";
            setData(path, ',', datasetCount);
            int riskper = 0;
            int res = 0;
            using (StreamReader reader = new StreamReader(path))
            {
                string curstr = reader.ReadLine();
                int mode = (int)modes.mi_fdch;
                for (int i = 0; i < datasetCount; i++)
                {
                    probDeathTestSet = model.ProbDisease(false,mode,
                    sex[i], age[i], sysbp[i], diabp[i], bpmeds[i], cursmoke[i], totchol[i], hdlc[i], ldlc[i], bmi[i], glucose[i],
                    diabetes[i], prevap[i], prevchd[i], prevmi[i], prevstrk[i], prevhyp[i], angina[i], hospmi[i], mi_fchd[i],
                    anychd[i], stroke[i], cvd[i], hyperten[i], death[i], randid[i], period[i], time[i],
                    timeap[i], timemi[i], timemifc[i], timechd[i], timestrk[i], timecvd[i], timehyp[i], timedth[i],
                    model.ProbSexPosterior, model.ProbBPMedsPosterior, model.CPTPrevAPPosterior, model.CPTPrevCHDPosterior,
                    model.CPTPrevMIPosterior, model.CPTPrevStrkPosterior, model.CPTPrevHypPosterior,
                    model.CPTAgePosterior, model.CPTHDLCPosterior, model.CPTGlucosePosterior, model.CPTDiabetesPosterior,
                    model.CPTBMIPosterior, model.CPTCurSmokePosterior, model.CPTDiaBPPosterior, model.CPTCVDPosterior,
                    model.CPTHypertenPosterior, model.CPTSysBPPosterior, model.CPTTotCholPosterior, model.CPTLDLCPosterior,
                    model.CPTStrokePosterior, model.CPTDeathPosterior, model.CPTAnginaPosterior, model.CPTAnyCHDPosterior,
                    model.CPTMI_FCHDPosterior, model.CPTHospMIPosterior,
                   null, model.ProbPerioddPosterior, model.CPTTimePosterior, model.CPTTimeAPPosterior, model.CPTTimeMIPosterior,
                    model.CPTTimeMIFCPosterior, model.CPTTimeCHDPosterior, model.CPTTimeCVDPosterior, model.CPTTimeSTRKPosterior, model.CPTTimeHypPosterior, model.CPTTimeDthPosterior
                    );
                    curstr = reader.ReadLine();
                    riskper = FraminghamRiskScore.risk(curstr, ',');
                    if (Math.Round(probDeathTestSet * 100)<=0 && riskper==0) res++;
                    else if (Math.Round(probDeathTestSet * 100)>=30 && riskper==30 ) res++;
                    else if (Math.Round(probDeathTestSet * 100) - 5 <= riskper && riskper <= Math.Round(probDeathTestSet * 100) + 5) res++;
                }
            }
            Console.WriteLine("Res: " + res + " %: " + ((double)res / (double)datasetCount));
            //Accuracy
           datasetCount = 3000;
           #region initDataSet
            randid=new int[datasetCount];
            period = new int[datasetCount];
           sex = new int[datasetCount];
           age = new int[datasetCount];
           time = new int[datasetCount];
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
           timeap = new int[datasetCount];
           timemi = new int[datasetCount];
           timemifc = new int[datasetCount];
           timechd = new int[datasetCount];
           timecvd = new int[datasetCount];
           timehyp = new int[datasetCount];
           timestrk = new int[datasetCount];
           death = new int[datasetCount];
           timedth = new int[datasetCount];

           #endregion
           path = @"D:\9-работа\helix\data\FRAMINGHAM_csv\frmgham3K.csv";
           setData(path, ',', datasetCount);

            int sumright=0;
            for (int i = 0; i < datasetCount; i++)
            {
                res = 0;
                int mode=(int)modes.death;
                probDeathTestSet = model.ProbDisease(true,mode,
                sex[i], age[i], sysbp[i], diabp[i], bpmeds[i], cursmoke[i], totchol[i], hdlc[i], ldlc[i], bmi[i], glucose[i],
                diabetes[i], prevap[i], prevchd[i], prevmi[i], prevstrk[i], prevhyp[i], angina[i], hospmi[i], mi_fchd[i],
                anychd[i], stroke[i], cvd[i], hyperten[i], death[i], randid[i], period[i], time[i],
                timeap[i], timemi[i], timemifc[i], timechd[i], timestrk[i], timecvd[i], timehyp[i], timedth[i],
                model.ProbSexPosterior, model.ProbBPMedsPosterior, model.CPTPrevAPPosterior, model.CPTPrevCHDPosterior,
                model.CPTPrevMIPosterior, model.CPTPrevStrkPosterior, model.CPTPrevHypPosterior,
                model.CPTAgePosterior, model.CPTHDLCPosterior, model.CPTGlucosePosterior, model.CPTDiabetesPosterior,
                model.CPTBMIPosterior, model.CPTCurSmokePosterior, model.CPTDiaBPPosterior, model.CPTCVDPosterior,
                model.CPTHypertenPosterior, model.CPTSysBPPosterior, model.CPTTotCholPosterior, model.CPTLDLCPosterior,
                model.CPTStrokePosterior, model.CPTDeathPosterior, model.CPTAnginaPosterior, model.CPTAnyCHDPosterior,
                model.CPTMI_FCHDPosterior, model.CPTHospMIPosterior,
               null, model.ProbPerioddPosterior, model.CPTTimePosterior, model.CPTTimeAPPosterior, model.CPTTimeMIPosterior,
                model.CPTTimeMIFCPosterior, model.CPTTimeCHDPosterior, model.CPTTimeCVDPosterior, model.CPTTimeSTRKPosterior, model.CPTTimeHypPosterior,  model.CPTTimeDthPosterior
                );
           

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

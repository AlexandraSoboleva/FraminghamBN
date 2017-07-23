using System;
using System.Collections.Generic;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;

using System.Linq;
using System.Text;


namespace FraminghamnBN
{
    #region usingVariableArraysNDepth<Vector/Dirichlet>
    using VarVectArr2 = VariableArray<VariableArray<Vector>, Vector[][]>;
    using VarVectArr3 = VariableArray<VariableArray<VariableArray<Vector>, Vector[][]>, Vector[][][]>;
    using VarVectArr4 = VariableArray<VariableArray<VariableArray<VariableArray<Vector>, Vector[][]>, Vector[][][]>, Vector[][][][]>;
    using VarVectArr5 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Vector>, Vector[][]>, Vector[][][]>, Vector[][][][]>, Vector[][][][][]>;
    using VarVectArr6 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Vector>, Vector[][]>, Vector[][][]>, Vector[][][][]>, Vector[][][][][]>, Vector[][][][][][]>;
    using VarVectArr8 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Vector>, Vector[][]>, Vector[][][]>, Vector[][][][]>, Vector[][][][][]>, Vector[][][][][][]>, Vector[][][][][][][]>, Vector[][][][][][][][]>;

    using VarDirArr2 = VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>;
    using VarDirArr3 = VariableArray<VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>, Dirichlet[][][]>;
    using VarDirArr4 = VariableArray<VariableArray<VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>, Dirichlet[][][]>, Dirichlet[][][][]>;
    using VarDirArr5 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>, Dirichlet[][][]>, Dirichlet[][][][]>, Dirichlet[][][][][]>;
    using VarDirArr6 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>, Dirichlet[][][]>, Dirichlet[][][][]>, Dirichlet[][][][][]>, Dirichlet[][][][][][]>;
    using VarDirArr8 = VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<VariableArray<Dirichlet>, Dirichlet[][]>, Dirichlet[][][]>, Dirichlet[][][][]>, Dirichlet[][][][][]>, Dirichlet[][][][][][]>, Dirichlet[][][][][][][]>, Dirichlet[][][][][][][][]>;
    
    #endregion
    class FraminghamModel
    {
       const int varCount=25;
        #region netVariables
        //Primary random variables
        //All variables have several states (if variable is continuos, ranges it's values)
        //Characteristics or risk factors
        public VariableArray<int> Sex;      //Participant sex    1=Men, 2=Women 
       // public VariableArray<int> Period;   //Examination Cycle   1=Period 1, 2=Period 2, 3=Period 3 
       // public VariableArray<int> Time;     //Number of days since baseline exam    0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> Age;      //Age at exam (years)   32-81: 1=32-41, 2=42-51, 3=52-61, 4=62-71, 5=72-81
        public VariableArray<int> SysBP;    //Systolic Blood Pressure (mean of last two of three measurements) (mmHg)  
                                            //83.5-295: 1=83.5-119, 2=119-154, 3=154-189, 4=189-224, 5=224-259
        public VariableArray<int> DiaBP;    //Diastolic Blood Pressure (mean of last two of three measurements) (mmHg)
                                            //30-150: 1=30-50, 2=51-70, 3=71-90, 4=91-110, 5=111-130, 6=131-150
        public VariableArray<int> BPMeds;   //Use of Anti-hypertensive medication at exam   0=Not currently used, 1=Current Use 
        public VariableArray<int> CurSmoke; //Current cigarette smoking at exam     0=Not current smoker, 1=Current smoker 
       // public VariableArray<int> CigDay;   //Number of cigarettes smoked each day  
                                            //0=Not current smoker, 1=1-30, 2=31-60, 3=61-90
       // public VariableArray<int> Educ;     //Attained Education    1=0-11 years, 2=High School Diploma, GED, 
                                            //                      3=Some College, Vocational School, 4=College (BS, BA) degree or more
        public VariableArray<int> TotChol;  //Serum Total Cholesterol (mg/dL)   107-696: 1=107-303, 2=304-499, 3=500-696
        public VariableArray<int> HDLC;     //High Density Lipoprotein Cholesterol (mg/dL)
                                            //10-189: 1=10-97, 2=98-129, 3=130-189
        public VariableArray<int> LDLC;     //Low Density Lipoprotein Cholesterol (mg/dL)
                                            //20-565: 1=20-201, 2=202-383, 3=384-565
        public VariableArray<int> BMI;      //Body Mass Index, weight in kilograms/height meters squared 
                                            //14.43-56.8: 1=14.43-20.48, 2=20.49-26.53, 3=26.54-32.58, 4=32.59-38.63
                                            // 5=38.64-44.68, 6=44.69-50.73, 7=50.4-56.8
        public VariableArray<int> Glucose;  //Casual serum glucose (mg/dL)  39-478: 1=39-259, 2=260-478
        public VariableArray<int> Diabetes; //Diabetic according to criteria of first exam treated or first exam with casual glucose of 200 mg/dL or more 
                                            //0=Not a diabetic, 1=Diabetic 
        //public VariableArray<int> HeartRte; //Heart rate (Ventricular rate) in beats/min 
                                            //37-220: 1=37-98; 2=99-159, 3=160-220
        public VariableArray<int> PrevAP;   //Prevalent Angina Pectoris at exam     0=Free of disease, 1=Prevalent disease 
        public VariableArray<int> PrevCHD;  //Prevalent Coronary Heart Disease defined as pre-existing Angina Pectoris, Myocardial Infarction (hospitalized, silent or unrecognized), or Coronary Insufficiency (unstable angina) 
                                            //0=Free of disease, 1=Prevalent disease 
        public VariableArray<int> PrevMI;   //Prevalent Myocardial Infarction       0=Free of disease, 1=Prevalent disease 
        public VariableArray<int> PrevStrk; //Prevalent Stroke     0=Free of disease, 1=Prevalent disease
        public VariableArray<int> PrevHyp;  //Prevalent Hypertensive. Subject was defined as hypertensive if treated or if second exam at which mean systolic was >=140 mmHg or mean Diastolic >=90 mmHg
                                            //0=Free of disease, 1=Prevalent disease 

        //Events
        public VariableArray<int> Angina;   //AnginaPectoris     0=Free of disease, 1=Prevalent disease
        public VariableArray<int> HospMI;  //Hospitalized Myocardial Infarction    0=Free of disease, 1=Prevalent disease
        public VariableArray<int> MI_FCHD;  //Hospitalized Myocardial Infarction or Fatal Coronary Heart Disease
                                            //0=Free of disease, 1=Prevalent disease
        public VariableArray<int> AnyCHD;   //Angina Pectoris, Myocardial infarction (Hospitalized and silent or unrecognized), Coronary Insufficiency (Unstable Angina), or Fatal Coronary Heart Disease   
                                            //0=Free of disease, 1=Prevalent disease
        public VariableArray<int> Stroke;   //Atherothrombotic infarction, Cerebral Embolism, Intracerebral Hemorrhage, or Subarachnoid Hemorrhage or Fatal Cerebrovascular Disease
                                            //0=Free of disease, 1=Prevalent disease
        public VariableArray<int> CVD;      //Myocardial infarction (Hospitalized and silent or unrecognized), Fatal Coronary Heart Disease, Atherothrombotic infarction, Cerebral Embolism, Intracerebral Hemorrhage, or Subarachnoid Hemorrhage or Fatal Cerebrovascular Disease 
                                            //0=Free of disease, 1=Prevalent disease
        public VariableArray<int> Hyperten;  //Hypertensive. Defined as the first exam treated for high blood pressure or second exam in which either Systolic is $ 140 mmHg or Diastolic $ 90mmHg 
                                            //0=Free of disease, 1=Prevalent disease
        public VariableArray<int> Death;    //Death from any cause      0=Free of disease, 1=Prevalent disease

        //TimeEvents
        /*
        public VariableArray<int> TimeAP;   //Number of days from Baseline exam to first Angina during the followup or Number of days from Baseline to censor date. Censor date may be end of followup, death or last known contact date if subject is lost to followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeMI;   //Defined as above for the first HOSPMI event during followup
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeMIFC; //Defined as above for the first MI_FCHD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeCHD;  //Defined as above for the first ANYCHD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeSTRK; //Defined as above for the first STROKE event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeCVD;  //Defined as above for the first CVD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeHYP;  //Defined as above for the first HYPERTEN event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeDTH;  //Number of days from Baseline exam to death if occurring during followup or Number of days from Baseline to censor date. Censor date may be end of followup, or last known contact date if subject is lost to followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        */
        #endregion
        public Variable<int> NumberOfExamples;

        #region Prob&CPT
        // Random variables representing the parameters of the distributions
        // of the primary random variables. For child variables, these are
        // in the form of conditional probability tables (CPTs)
        //None parent
        public Variable<Vector> ProbSex;
        public Variable<Vector> ProbBPMeds;
        //Consider that these parametrs do not depend on others, we do not know why they are appear
        public Variable<Vector> ProbPrevAP;
        public Variable<Vector> ProbPrevCHD;
        public Variable<Vector> ProbPrevMI;
        public Variable<Vector> ProbPrevStrk;
        public Variable<Vector> ProbPrevHyp;
        //One parent
        public VariableArray<Vector> CPTAge;        //parent: Sex
        public VariableArray<Vector> CPTHDLC;       //parent: BMI
        public VariableArray<Vector> CPTGlucose;    //parent: Diabetes
        public VariableArray<Vector> CPTDiabetes;   //parent: BMI     
        //Two parents
        public VarVectArr2 CPTBMI;         //parents: Sex, Age
        public VarVectArr2 CPTCurSmoke;    //parents: Sex, Age
        //Three parents
        public VarVectArr3 CPTDiaBP;       //parents: SysBP, Age, CVD
        public VarVectArr3 CPTCVD;         //parents: PrevCHD, CurSmoke, PrevMI
        public VarVectArr3 CPTHyperten;    //parents: Age, PrevHyp, DiaBP
        //Four parents
        public VarVectArr4 CPTSysBP;       //parents: BMI, Age, CurSmoke, BPMeds
        public VarVectArr4 CPTTotChol;     //parents: Sex, Age, HDLC, LDLC
        public VarVectArr4 CPTLDLC;        //parents: BMI, Diabetes, CurSmoke, Hyperten
        //Five parents
        public VarVectArr5 CPTStroke;      //parents: PrevStrk, AnyCHD, DiaBP, CurSmoke, Diabetes
        public VarVectArr5 CPTDeath;       //parents: Age, HospMI, MI_FCHD, Stroke, AnyCHD
        //Six parents
        public VarVectArr6 CPTAngina;      //parents: PrevAp, Diabetes, DiaBP, CurSmoke, Sex, Age
        public VarVectArr6 CPTAnyCHD;     //parents: PrevCHD, HospMI, Angina, DiaBP, CVD, Diabetes
        //Eight parents
        public VarVectArr8 CPTMI_FCHD;     //parents: PrevMI, PrevCHD, Diabetes, DiaBP, TotChol, CVD, Sex, Age
        public VarVectArr8 CPTHospMI;      //parents: PrevMI, Diabetes, DiaBP, TotChol, Angina, Sex, Age, MI_FCHD

        #endregion

        #region PriorProb&CPT
        // Prior distributions for the probability and CPT variables.
        // The prior distributions are formulated as Infer.NET variables
        // so that they can be set at runtime without recompiling the model
        public Variable<Dirichlet> ProbSexPrior;
        public Variable<Dirichlet> ProbBPMedsPrior;
        public Variable<Dirichlet> ProbPrevAPPrior;
        public Variable<Dirichlet> ProbPrevCHDPrior;
        public Variable<Dirichlet> ProbPrevMIPrior;
        public Variable<Dirichlet> ProbPrevStrkPrior;
        public Variable<Dirichlet> ProbPrevHypPrior;
        public VariableArray<Dirichlet> CPTAgePrior;
        public VariableArray<Dirichlet> CPTHDLCPrior;
        public VariableArray<Dirichlet> CPTGlucosePrior;
        public VariableArray<Dirichlet> CPTDiabetesPrior;
        public VarDirArr2 CPTBMIPrior;
        public VarDirArr2 CPTCurSmokePrior;
        public VarDirArr3 CPTDiaBPPrior;
        public VarDirArr3 CPTCVDPrior;
        public VarDirArr3 CPTHypertenPrior;
        public VarDirArr4 CPTSysBPPrior;
        public VarDirArr4 CPTTotCholPrior;
        public VarDirArr4 CPTLDLCPrior;
        public VarDirArr5 CPTStrokePrior;
        public VarDirArr5 CPTDeathPrior;
        public VarDirArr6 CPTAnginaPrior;
        public VarDirArr6 CPTAnyCHDPrior;
        public VarDirArr8 CPTMI_FCHDPrior;
        public VarDirArr8 CPTHospMIPrior;

        #endregion

        #region PosteriorProb&CPT
        // Posterior distributions for the probability and CPT variables
        public Dirichlet ProbSexPosterior;
        public Dirichlet ProbBPMedsPosterior;
        public Dirichlet ProbPrevAPPosterior;
        public Dirichlet ProbPrevCHDPosterior;
        public Dirichlet ProbPrevMIPosterior;
        public Dirichlet ProbPrevStrkPosterior;
        public Dirichlet ProbPrevHypPosterior;
        public Dirichlet[] CPTAgePosterior;
        public Dirichlet[] CPTHDLCPosterior;
        public Dirichlet[] CPTGlucosePosterior;
        public Dirichlet[] CPTDiabetesPosterior;
        public Dirichlet[][] CPTBMIPosterior;
        public Dirichlet[][] CPTCurSmokePosterior;
        public Dirichlet[][][] CPTDiaBPPosterior;
        public Dirichlet[][][] CPTCVDPosterior;
        public Dirichlet[][][] CPTHypertenPosterior;
        public Dirichlet[][][][] CPTSysBPPosterior;
        public Dirichlet[][][][] CPTTotCholPosterior;
        public Dirichlet[][][][] CPTLDLCPosterior;
        public Dirichlet[][][][][] CPTStrokePosterior;
        public Dirichlet[][][][][] CPTDeathPosterior;
        public Dirichlet[][][][][][] CPTAnginaPosterior;
        public Dirichlet[][][][][][] CPTAnyCHDPosterior;
        public Dirichlet[][][][][][][][] CPTMI_FCHDPosterior;
        public Dirichlet[][][][][][][][] CPTHospMIPosterior;

        #endregion

        // Inference engine
        public InferenceEngine Engine = new InferenceEngine();


        //Constructor
        public FraminghamModel()
        {
            // Set up the ranges
            NumberOfExamples = Variable.New<int>().Named("NofE");
            Range N = new Range(NumberOfExamples).Named("N");

            #region SetUpRanges
            Range RSex = new Range(2).Named("RSex");
            Range RAge = new Range(5).Named("RAge");
            Range RSysBP = new Range(5).Named("RSysBp");
            Range RDiaBP = new Range(6).Named("RDiaBP");
            Range RBPMeds= new Range(2).Named("RBPMeds");
            Range RCurSmoke = new Range(2).Named("RCurSmoke");
            Range RTotChol = new Range(3).Named("RTotChol");
            Range RHDLC = new Range(3).Named("RHDLC");
            Range RLDLC = new Range(3).Named("RLDLC");
            Range RBMI = new Range(7).Named("RBMI");
            Range RGlucose = new Range(2).Named("RGlucose");
            Range RDiabetes = new Range(2).Named("RDiabetes");
            Range RPrevAP = new Range(2).Named("RPrevAP");
            Range RPrevCHD = new Range(2).Named("RPrevCHD");
            Range RPrevMI = new Range(2).Named("RPrevMI");
            Range RPrevStrk = new Range(2).Named("RPrevStrk");
            Range RPrevHyp = new Range(2).Named("RPrevHyp");
            Range RAngina = new Range(2).Named("RAngina");
            Range RHospMI = new Range(2).Named("RHospMI");
            Range RMI_FCHD = new Range(2).Named("RMI_FCHD");
            Range RAnyCHD = new Range(2).Named("RAnyCHD");
            Range RStroke = new Range(2).Named("RStroke");
            Range RCVD = new Range(2).Named("RCVD");
            Range RHyperten = new Range(2).Named("RHyperten");
            Range RDeath = new Range(2).Named("RDeath");

            #endregion

            #region DefinePriors
            // Define the priors and the parameters
            ProbSexPrior = Variable.New<Dirichlet>().Named("ProbSexPrior");
            ProbSex = Variable<Vector>.Random(ProbSexPrior).Named("ProbSex");
            ProbSex.SetValueRange(RSex);
            
            ProbBPMedsPrior = Variable.New<Dirichlet>().Named("ProbBPMedsPrior");
            ProbBPMeds = Variable<Vector>.Random(ProbBPMedsPrior).Named("ProbBPMeds");
            ProbBPMeds.SetValueRange(RBPMeds);
            
            ProbPrevAPPrior = Variable.New<Dirichlet>().Named("ProbPrevAPPrior");
            ProbPrevAP = Variable<Vector>.Random(ProbPrevAPPrior).Named("ProbPrevAP");
            ProbPrevAP.SetValueRange(RPrevAP);
           
            ProbPrevCHDPrior = Variable.New<Dirichlet>().Named("ProbPrevCHDPrior");
            ProbPrevCHD = Variable<Vector>.Random(ProbPrevCHDPrior).Named("ProbPrevCHD");
            ProbPrevCHD.SetValueRange(RPrevCHD);
           
            ProbPrevMIPrior = Variable.New<Dirichlet>().Named("ProbPrevMIPrior");
            ProbPrevMI = Variable<Vector>.Random(ProbPrevMIPrior).Named("ProbPrevMI");
            ProbPrevMI.SetValueRange(RPrevMI);
           
            ProbPrevStrkPrior = Variable.New<Dirichlet>().Named("ProbPrevStrkPrior");
            ProbPrevStrk = Variable<Vector>.Random(ProbPrevStrkPrior).Named("ProbPrevStrk");
            ProbPrevStrk.SetValueRange(RPrevStrk);
            
            ProbPrevHypPrior = Variable.New<Dirichlet>().Named("ProbPrevHypPrior");
            ProbPrevHyp = Variable<Vector>.Random(ProbPrevHypPrior).Named("ProbPrevHyp");
            ProbPrevHyp.SetValueRange(RPrevHyp);

            CPTAgePrior = Variable.Array<Dirichlet>(RSex).Named("CPTAgePrior");
            CPTAge = Variable.Array<Vector>(RSex).Named("CPTAge");
            CPTAge[RSex] = Variable<Vector>.Random(CPTAgePrior[RSex]);
            CPTAge.SetValueRange(RAge);

            CPTBMIPrior = Variable.Array(Variable.Array<Dirichlet>(RSex), RAge).Named("CPTBMIPrior");
            CPTBMI = Variable.Array(Variable.Array<Vector>(RSex), RAge).Named("CPTBMI");
            CPTBMI[RAge][RSex] = Variable<Vector>.Random(CPTBMIPrior[RAge][RSex]);
            CPTBMI.SetValueRange(RBMI);

            CPTCurSmokePrior = Variable.Array(Variable.Array<Dirichlet>(RSex), RAge).Named("CPTCurSmokePrior");
            CPTCurSmoke = Variable.Array(Variable.Array<Vector>(RSex), RAge).Named("CPTCurSmoke");
            CPTCurSmoke[RAge][RSex] = Variable<Vector>.Random(CPTCurSmokePrior[RAge][RSex]);
            CPTCurSmoke.SetValueRange(RCurSmoke);

            CPTHDLCPrior = Variable.Array<Dirichlet>(RBMI).Named("CPTHDLCPrior");
            CPTHDLC = Variable.Array<Vector>(RBMI).Named("CPTHDLC");
            CPTHDLC[RBMI] = Variable<Vector>.Random(CPTHDLCPrior[RBMI]);
            CPTHDLC.SetValueRange(RHDLC);

            CPTDiabetesPrior = Variable.Array<Dirichlet>(RBMI).Named("CPTDiabetesPrior");
            CPTDiabetes = Variable.Array<Vector>(RBMI).Named("CPTDiabetes");
            CPTDiabetes[RBMI] = Variable<Vector>.Random(CPTDiabetesPrior[RBMI]);
            CPTDiabetes.SetValueRange(RDiabetes);

            CPTGlucosePrior = Variable.Array<Dirichlet>(RDiabetes).Named("CPTGlucosePrior");
            CPTGlucose = Variable.Array<Vector>(RDiabetes).Named("CPTGlucose");
            CPTGlucose[RDiabetes] = Variable<Vector>.Random(CPTGlucosePrior[RDiabetes]);
            CPTGlucose.SetValueRange(RGlucose);
            
            CPTCVDPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RPrevCHD), RCurSmoke), RPrevMI).Named("CPTCVDPrior");
            CPTCVD = Variable.Array(Variable.Array(Variable.Array<Vector>(RPrevCHD), RCurSmoke), RPrevMI).Named("CPTCVD");
            CPTCVD[RPrevMI][RCurSmoke][RPrevCHD] = Variable<Vector>.Random(CPTCVDPrior[RPrevMI][RCurSmoke][RPrevCHD]);
            CPTCVD.SetValueRange(RCVD);

            CPTSysBPPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RBMI), RCurSmoke), RBPMeds).Named("CPTSysBPPrior");
            CPTSysBP = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RBMI), RCurSmoke), RBPMeds).Named("CPTSysBP");
            CPTSysBP[RBPMeds][RCurSmoke][RBMI][RAge] = Variable<Vector>.Random(CPTSysBPPrior[RBPMeds][RCurSmoke][RBMI][RAge]);
            CPTSysBP.SetValueRange(RSysBP);

            CPTDiaBPPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RSysBP), RAge), RCVD).Named("CPTDiaBPPrior");
            CPTDiaBP = Variable.Array(Variable.Array(Variable.Array<Vector>(RSysBP), RAge), RCVD).Named("CPTDiaBP");
            CPTDiaBP[RCVD][RAge][RSysBP] = Variable<Vector>.Random(CPTDiaBPPrior[RCVD][RAge][RSysBP]);
            CPTDiaBP.SetValueRange(RDiaBP);

            CPTHypertenPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RPrevHyp), RDiaBP).Named("CPTHypertenPrior");
            CPTHyperten = Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RPrevHyp), RDiaBP).Named("CPTHyperten");
            CPTHyperten[RDiaBP][RPrevHyp][RAge] = Variable<Vector>.Random(CPTHypertenPrior[RDiaBP][RPrevHyp][RAge]);
            CPTHyperten.SetValueRange(RHyperten);

            CPTLDLCPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RDiabetes), RBMI), RCurSmoke), RHyperten).Named("CPTLDLCPrior");
            CPTLDLC = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RDiabetes), RBMI), RCurSmoke), RHyperten).Named("CPTLDLC");
            CPTLDLC[RHyperten][RCurSmoke][RBMI][RDiabetes] = Variable<Vector>.Random(CPTLDLCPrior[RHyperten][RCurSmoke][RBMI][RDiabetes]);
            CPTLDLC.SetValueRange(RLDLC);

            CPTTotCholPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RSex), RHDLC), RLDLC).Named("CPTTotCholPrior");
            CPTTotChol = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RSex), RHDLC), RLDLC).Named("CPTTotChol");
            CPTTotChol[RLDLC][RHDLC][RSex][RAge] = Variable<Vector>.Random(CPTTotCholPrior[RLDLC][RHDLC][RSex][RAge]);
            CPTTotChol.SetValueRange(RTotChol);

            CPTAnginaPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RDiabetes), RDiaBP), RCurSmoke), RSex), RPrevAP).Named("CPTAnginaPrior");
            CPTAngina = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RDiabetes), RDiaBP), RCurSmoke), RSex), RPrevAP).Named("CPTAngina");
            CPTAngina[RPrevAP][RSex][RCurSmoke][RDiaBP][RDiabetes][RAge] = Variable<Vector>.Random(CPTAnginaPrior[RPrevAP][RSex][RCurSmoke][RDiaBP][RDiabetes][RAge]);
            CPTAngina.SetValueRange(RAngina);

            CPTMI_FCHDPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RPrevCHD), RDiabetes), RDiaBP), RPrevMI), RTotChol), RCVD), RSex), RAge).Named("CPTMI_FCHDPrior");
            CPTMI_FCHD = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RPrevCHD), RDiabetes), RDiaBP), RPrevMI), RTotChol), RCVD), RSex), RAge).Named("CPTMI_FCHD");
            CPTMI_FCHD[RAge][RSex][RCVD][RTotChol][RPrevMI][RDiaBP][RDiabetes][RPrevCHD] = Variable<Vector>.Random(CPTMI_FCHDPrior[RAge][RSex][RCVD][RTotChol][RPrevMI][RDiaBP][RDiabetes][RPrevCHD]);
            CPTMI_FCHD.SetValueRange(RMI_FCHD);

            CPTHospMIPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAngina), RDiabetes), RDiaBP), RPrevMI), RTotChol), RMI_FCHD), RSex), RAge).Named("CPTHospMIPrior");
            CPTHospMI = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAngina), RDiabetes), RDiaBP), RPrevMI), RTotChol), RMI_FCHD), RSex), RAge).Named("CPTHospMI");
            CPTHospMI[RAge][RSex][RMI_FCHD][RTotChol][RPrevMI][RDiaBP][RDiabetes][RAngina] = Variable<Vector>.Random(CPTHospMIPrior[RAge][RSex][RMI_FCHD][RTotChol][RPrevMI][RDiaBP][RDiabetes][RAngina]);
            CPTHospMI.SetValueRange(RHospMI);


            CPTAnyCHDPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RPrevCHD), RDiabetes), RDiaBP), RHospMI), RAngina), RCVD).Named("CPTAnyCHDPrior");
            CPTAnyCHD = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RPrevCHD), RDiabetes), RDiaBP), RHospMI), RAngina), RCVD).Named("CPTAnyCHD");
            CPTAnyCHD[RCVD][RAngina][RHospMI][RDiaBP][RDiabetes][RPrevCHD] = Variable<Vector>.Random(CPTAnyCHDPrior[RCVD][RAngina][RHospMI][RDiaBP][RDiabetes][RPrevCHD]);
            CPTAnyCHD.SetValueRange(RAnyCHD);

            CPTStrokePrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RPrevStrk), RAnyCHD), RCurSmoke), RDiaBP), RDiabetes).Named("CPTStrokePrior");
            CPTStroke = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RPrevStrk), RAnyCHD), RCurSmoke), RDiaBP), RDiabetes).Named("CPTStroke");
            CPTStroke[RDiabetes][RDiaBP][RCurSmoke][RAnyCHD][RPrevStrk] = Variable<Vector>.Random(CPTStrokePrior[RDiabetes][RDiaBP][RCurSmoke][RAnyCHD][RPrevStrk]);
            CPTStroke.SetValueRange(RStroke);

            CPTDeathPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RAnyCHD), RHospMI), RMI_FCHD), RStroke).Named("CPTDeathPrior");
            CPTDeath = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RAnyCHD), RHospMI), RMI_FCHD), RStroke).Named("CPTDeath");
            CPTDeath[RStroke][RMI_FCHD][RHospMI][RAnyCHD][RAge] = Variable<Vector>.Random(CPTDeathPrior[RStroke][RMI_FCHD][RHospMI][RAnyCHD][RAge]);
            CPTDeath.SetValueRange(RDeath);
           
            #endregion

            #region DefinePrimaryVars
            // Define the primary variables
            Sex = Variable.Array<int>(N).Named("Sex");
            Sex[N] = Variable.Discrete(ProbSex).ForEach(N);
            BPMeds = Variable.Array<int>(N).Named("BPMeds");
            BPMeds[N] = Variable.Discrete(ProbBPMeds).ForEach(N);
            PrevAP = Variable.Array<int>(N).Named("PrevAP");
            PrevAP[N] = Variable.Discrete(ProbPrevAP).ForEach(N);
            PrevCHD = Variable.Array<int>(N).Named("PrevCHD");
            PrevCHD[N] = Variable.Discrete(ProbPrevCHD).ForEach(N);
            PrevMI = Variable.Array<int>(N).Named("PrevMI");
            PrevMI[N] = Variable.Discrete(ProbPrevMI).ForEach(N);
            PrevStrk = Variable.Array<int>(N).Named("PrevStrk");
            PrevStrk[N] = Variable.Discrete(ProbPrevStrk).ForEach(N);
            PrevHyp = Variable.Array<int>(N).Named("PrevHyp");
            PrevHyp[N] = Variable.Discrete(ProbPrevHyp).ForEach(N);

            Age = AddChildFromOneParent(Sex,CPTAge).Named("Age");
            BMI = AddChildFromTwoParents(Age,Sex, CPTBMI).Named("BMI");
            CurSmoke = AddChildFromTwoParents(Age, Sex, CPTCurSmoke).Named("CurSmoke");
            HDLC = AddChildFromOneParent(BMI, CPTHDLC).Named("HDLC");
            Diabetes = AddChildFromOneParent(BMI, CPTDiabetes).Named("Diabetes");
            Glucose = AddChildFromOneParent(Diabetes, CPTGlucose).Named("Glucose");
            CVD = AddChildFromThreeParents(PrevMI, CurSmoke, PrevCHD, CPTCVD).Named("CVD");
            SysBP = AddChildFromFourParents(BPMeds, CurSmoke, BMI, Age, CPTSysBP).Named("SysBP");
            DiaBP = AddChildFromThreeParents(CVD, Age,SysBP, CPTDiaBP).Named("DiaBP");
            Hyperten = AddChildFromThreeParents(DiaBP, PrevHyp, Age, CPTHyperten).Named("Hyperten");
            LDLC = AddChildFromFourParents(Hyperten, CurSmoke, BMI, Diabetes, CPTLDLC).Named("LDLC");
            TotChol = AddChildFromFourParents(LDLC, HDLC, Sex, Age, CPTTotChol).Named("TotChol");
            Angina = AddChildFromSixParents(PrevAP, Sex, CurSmoke, DiaBP, Diabetes, Age, CPTAngina).Named("Angina");
            MI_FCHD = AddChildFromEightParents(Age, Sex, CVD, TotChol, PrevMI, DiaBP, Diabetes, PrevCHD, CPTMI_FCHD).Named("MI_FCHD");
            HospMI = AddChildFromEightParents(Age, Sex, MI_FCHD, TotChol, PrevMI, DiaBP, Diabetes, Angina, CPTHospMI).Named("HospMI");
            AnyCHD = AddChildFromSixParents(CVD, Angina, HospMI, DiaBP, Diabetes, PrevCHD, CPTAnyCHD).Named("AnyCHD");
            Stroke = AddChildFromFiveParents(Diabetes, DiaBP, CurSmoke, AnyCHD, PrevStrk, CPTStroke).Named("Stroke");
            Death = AddChildFromFiveParents(Stroke, MI_FCHD, HospMI, AnyCHD, Age, CPTDeath).Named("Death");

            #endregion
        }

        //Learning
        public void LearnParameters(int count,
            int[] sex, int[] age, int[] sysbp, int[] diabp, int[] bpmeds, int[] cursmoke, int[]totchol, int[] hdlc, int[] ldlc,
            int[] bmi, int[] glucose, int[] diabetes, int[]prevap, int[] prevchd, int[] prevmi, int[] prevstrk, int[] prevhyp,
            int[] angina, int[] hospmi, int[] mi_fchd, int[] anychd, int[] stroke, int[] cvd, int[] hyperten, int[] death,
            Dirichlet probSexPrior, Dirichlet probBPMedsPrior, Dirichlet probPrevAPPrior, Dirichlet probPrevCHDPrior, Dirichlet probPrevMIPrior,
            Dirichlet probPrevStrkPrior, Dirichlet probPrevHypPrior,
            Dirichlet[] cptAgePrior, Dirichlet[] cptHDLCPrior, Dirichlet[] cptGlucosePrior, Dirichlet[] cptDiabetesPrior,
            Dirichlet[][] cptBMIPrior, Dirichlet[][] cptCurSmokePrior, 
            Dirichlet[][][] cptDiaBPPrior, Dirichlet[][][] cptCVDPrior, Dirichlet[][][] cptHypertenPrior,
            Dirichlet[][][][] cptSysBPPrior, Dirichlet[][][][] cptTotCholPrior, Dirichlet[][][][] cptLDLCPrior,
            Dirichlet[][][][][] cptStrokePrior, Dirichlet[][][][][] cptDeathPrior,
            Dirichlet[][][][][][] cptAnginaPrior, Dirichlet[][][][][][] cptAnyCHDPrior,
            Dirichlet[][][][][][][][] cptMI_FCHDPrior, Dirichlet[][][][][][][][] cptHospMIPrior
            )
        {
            NumberOfExamples.ObservedValue = count;
           
            #region PrimaryVarObservedValue
            Sex.ObservedValue = sex;
            BPMeds.ObservedValue = bpmeds;
            PrevAP.ObservedValue = prevap;
            PrevCHD.ObservedValue = prevchd;
            PrevMI.ObservedValue = prevmi;
            PrevStrk.ObservedValue = prevstrk;
            PrevHyp.ObservedValue = prevhyp;
            Age.ObservedValue = age;
            BMI.ObservedValue = bmi;
            CurSmoke.ObservedValue = cursmoke;
            HDLC.ObservedValue = hdlc;
            Diabetes.ObservedValue = diabetes;
            Glucose.ObservedValue = glucose;
            CVD.ObservedValue = cvd;
            SysBP.ObservedValue = sysbp;
            DiaBP.ObservedValue = diabp;
            Hyperten.ObservedValue = hyperten;
            LDLC.ObservedValue = ldlc;
            TotChol.ObservedValue = totchol;
            Angina.ObservedValue = angina;
            MI_FCHD.ObservedValue = mi_fchd;
            HospMI.ObservedValue = hospmi;
            AnyCHD.ObservedValue = anychd;
            Stroke.ObservedValue = stroke;
            Death.ObservedValue = death;
            
            #endregion

            #region PriorProb&CPTObservedValue
            ProbSexPrior.ObservedValue = probSexPrior;
            ProbBPMedsPrior.ObservedValue = probBPMedsPrior;
            ProbPrevAPPrior.ObservedValue = probPrevAPPrior;
            ProbPrevCHDPrior.ObservedValue = probPrevCHDPrior;
            ProbPrevMIPrior.ObservedValue = probPrevMIPrior;
            ProbPrevStrkPrior.ObservedValue = probPrevStrkPrior;
            ProbPrevHypPrior.ObservedValue = probPrevHypPrior;

            CPTAgePrior.ObservedValue = cptAgePrior;
            CPTBMIPrior.ObservedValue = cptBMIPrior;
            CPTCurSmokePrior.ObservedValue = cptCurSmokePrior;
            CPTHDLCPrior.ObservedValue = cptHDLCPrior;
            CPTDiabetesPrior.ObservedValue = cptDiabetesPrior;
            CPTGlucosePrior.ObservedValue = cptGlucosePrior;
            CPTCVDPrior.ObservedValue = cptCVDPrior;
            CPTSysBPPrior.ObservedValue = cptSysBPPrior;
            CPTDiaBPPrior.ObservedValue = cptDiaBPPrior;
            CPTHypertenPrior.ObservedValue = cptHypertenPrior;
            CPTLDLCPrior.ObservedValue = cptLDLCPrior;
            CPTTotCholPrior.ObservedValue = cptTotCholPrior;
            CPTAnginaPrior.ObservedValue = cptAnginaPrior;
            CPTMI_FCHDPrior.ObservedValue = cptMI_FCHDPrior;
            CPTHospMIPrior.ObservedValue = cptHospMIPrior;
            CPTAnyCHDPrior.ObservedValue = cptAnyCHDPrior;
            CPTStrokePrior.ObservedValue = cptStrokePrior;
            CPTDeathPrior.ObservedValue = cptDeathPrior;

            #endregion

            // Inference
            ProbSexPosterior = Engine.Infer<Dirichlet>(ProbSex);
            ProbBPMedsPosterior = Engine.Infer<Dirichlet>(ProbBPMeds);
            ProbPrevAPPosterior = Engine.Infer<Dirichlet>(ProbPrevAP);
            ProbPrevCHDPosterior = Engine.Infer<Dirichlet>(ProbPrevCHD);
            ProbPrevMIPosterior = Engine.Infer<Dirichlet>(ProbPrevMI);
            ProbPrevStrkPosterior = Engine.Infer<Dirichlet>(ProbPrevStrk);
            ProbPrevHypPosterior = Engine.Infer<Dirichlet>(ProbPrevHyp);

            CPTAgePosterior = Engine.Infer<Dirichlet[]>(CPTAge);
            CPTBMIPosterior = Engine.Infer<Dirichlet[][]>(CPTBMI);
            CPTCurSmokePosterior = Engine.Infer<Dirichlet[][]>(CPTCurSmoke);
            CPTHDLCPosterior = Engine.Infer<Dirichlet[]>(CPTHDLC);
            CPTDiabetesPosterior = Engine.Infer<Dirichlet[]>(CPTDiabetes);
            CPTGlucosePosterior = Engine.Infer<Dirichlet[]>(CPTGlucose);
            CPTCVDPosterior = Engine.Infer<Dirichlet[][][]>(CPTCVD);
            CPTSysBPPosterior = Engine.Infer<Dirichlet[][][][]>(CPTSysBP);
            CPTDiaBPPosterior = Engine.Infer<Dirichlet[][][]>(CPTDiaBP);
            CPTHypertenPosterior = Engine.Infer<Dirichlet[][][]>(CPTHyperten);
            CPTLDLCPosterior = Engine.Infer<Dirichlet[][][][]>(CPTLDLC);
            CPTTotCholPosterior = Engine.Infer<Dirichlet[][][][]>(CPTTotChol);
            CPTAnginaPosterior = Engine.Infer<Dirichlet[][][][][][]>(CPTAngina);
            CPTMI_FCHDPosterior = Engine.Infer<Dirichlet[][][][][][][][]>(CPTMI_FCHD);
            CPTHospMIPosterior = Engine.Infer<Dirichlet[][][][][][][][]>(CPTHospMI);
            CPTAnyCHDPosterior = Engine.Infer<Dirichlet[][][][][][]>(CPTAnyCHD);
            CPTStrokePosterior = Engine.Infer<Dirichlet[][][][][]>(CPTStroke);
            CPTDeathPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTDeath);
        }

        public void LearnParameters(int count,
            int[] sex, int[] age, int[] sysbp, int[] diabp, int[] bpmeds, int[] cursmoke, int[]totchol, int[] hdlc, int[] ldlc,
            int[] bmi, int[] glucose, int[] diabetes, int[]prevap, int[] prevchd, int[] prevmi, int[] prevstrk, int[] prevhyp,
            int[] angina, int[] hospmi, int[] mi_fchd, int[] anychd, int[] stroke, int[] cvd, int[] hyperten, int[] death
            )
        {
            // Set all priors to uniform
            Dirichlet probSexPrior = Dirichlet.Uniform(2);
            Dirichlet probBPMedsPrior = Dirichlet.Uniform(2);
            Dirichlet probPrevAPPrior = Dirichlet.Uniform(2);
            Dirichlet probPrevCHDPrior = Dirichlet.Uniform(2);
            Dirichlet probPrevMIPrior = Dirichlet.Uniform(2);
            Dirichlet probPrevStrkPrior = Dirichlet.Uniform(2);
            Dirichlet probPrevHypPrior = Dirichlet.Uniform(2);

            Dirichlet[] cptAgePrior = Enumerable.Repeat(Dirichlet.Uniform(5), 2).ToArray();         //5 2
            Dirichlet[][] cptBMIPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(7), 2).ToArray(), 5).ToArray();     //7 5 2                
            Dirichlet[][] cptCurSmokePrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 5).ToArray(); ; //2 2 5
            Dirichlet[] cptHDLCPrior = Enumerable.Repeat(Dirichlet.Uniform(3), 7).ToArray();        //3 7                     
            Dirichlet[] cptDiabetesPrior = Enumerable.Repeat(Dirichlet.Uniform(2), 7).ToArray();    //2 7  
            Dirichlet[] cptGlucosePrior = Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray();     //2 2                  
            Dirichlet[][][] cptCVDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 2).ToArray();     //2 2 2 2        
            Dirichlet[][][][] cptSysBPPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(5), 5).ToArray(), 7).ToArray(), 2).ToArray(), 2).ToArray();  //5 2 7 5 2                   
            Dirichlet[][][] cptDiaBPPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 5).ToArray(), 5).ToArray(), 2).ToArray();   //6 5 5 2        
            Dirichlet[][][] cptHypertenPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 2).ToArray(), 6).ToArray(); //2 2 5 6
            Dirichlet[][][][] cptLDLCPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(3), 2).ToArray(), 7).ToArray(), 2).ToArray(), 2).ToArray();     //3 7 2 2 2                 
            Dirichlet[][][][] cptTotCholPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(3), 5).ToArray(), 2).ToArray(), 3).ToArray(), 3).ToArray(); //3 2 5 3 3                  
            Dirichlet[][][][][][] cptAnginaPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray();   //2 2 2 2 2 5 6
            Dirichlet[][][][][][][][] cptMI_FCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 3).ToArray(), 2).ToArray(), 2).ToArray(), 5).ToArray();//2 2 2 2 2 5 5 3 2
            Dirichlet[][][][][][][][] cptHospMIPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 3).ToArray(), 2).ToArray(), 2).ToArray(), 5).ToArray();    //2 2 2 2 2 5 6 3 2
            Dirichlet[][][][][][] cptAnyCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray();  //2 2 2 2 2 6 2          
            Dirichlet[][][][][] cptStrokePrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(); //2 2 2 2 2 6
            Dirichlet[][][][][] cptDeathPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray();   // 2 2 2 2 2 5             
            

            LearnParameters(count,sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, glucose, diabetes,
                prevap, prevchd, prevmi, prevstrk, prevhyp, angina, hospmi, mi_fchd, anychd, stroke, cvd, hyperten, death,
                probSexPrior, probBPMedsPrior, probPrevAPPrior, probPrevCHDPrior, probPrevMIPrior, probPrevStrkPrior, probPrevHypPrior,
                cptAgePrior, cptHDLCPrior, cptGlucosePrior, cptDiabetesPrior, cptBMIPrior, cptCurSmokePrior, cptDiaBPPrior,
                cptCVDPrior, cptHypertenPrior, cptSysBPPrior, cptTotCholPrior, cptLDLCPrior, cptStrokePrior,
                cptDeathPrior, cptAnginaPrior, cptAnyCHDPrior, cptMI_FCHDPrior, cptHospMIPrior
                );     
        }
                                                                    
        //Probility 
        //mode - which probability is need                                       
        public double ProbDisease(int mode,
            int? sex, int? age, int? sysbp, int? diabp, int? bpmeds, int? cursmoke, int? totchol, int? hdlc, int? ldlc,
            int? bmi, int? glucose, int? diabetes, int? prevap, int? prevchd, int? prevmi, int? prevstrk, int? prevhyp,
            int? angina, int? hospmi, int? mi_fchd, int? anychd, int? stroke, int? cvd, int? hyperten, int? death,
            Dirichlet probSexPrior, Dirichlet probBPMedsPrior, Dirichlet probPrevAPPrior, Dirichlet probPrevCHDPrior, Dirichlet probPrevMIPrior,
            Dirichlet probPrevStrkPrior, Dirichlet probPrevHypPrior,
            Dirichlet[] cptAgePrior, Dirichlet[] cptHDLCPrior, Dirichlet[] cptGlucosePrior, Dirichlet[] cptDiabetesPrior,
            Dirichlet[][] cptBMIPrior, Dirichlet[][] cptCurSmokePrior,
            Dirichlet[][][] cptDiaBPPrior, Dirichlet[][][] cptCVDPrior, Dirichlet[][][] cptHypertenPrior,
            Dirichlet[][][][] cptSysBPPrior, Dirichlet[][][][] cptTotCholPrior, Dirichlet[][][][] cptLDLCPrior,
            Dirichlet[][][][][] cptStrokePrior, Dirichlet[][][][][] cptDeathPrior,
            Dirichlet[][][][][][] cptAnginaPrior, Dirichlet[][][][][][] cptAnyCHDPrior,
            Dirichlet[][][][][][][][] cptMI_FCHDPrior, Dirichlet[][][][][][][][] cptHospMIPrior
            )
        {
            NumberOfExamples.ObservedValue = 1;

            #region SetOrClearObservedValue

            if (sex.HasValue) Sex.ObservedValue = new int[] { sex.Value };  else Sex.ClearObservedValue();
            if (bpmeds.HasValue) BPMeds.ObservedValue = new int[] { bpmeds.Value }; else BPMeds.ClearObservedValue();
            if (prevap.HasValue) PrevAP.ObservedValue = new int[] { prevap.Value }; else PrevAP.ClearObservedValue();
            if (prevchd.HasValue) PrevCHD.ObservedValue = new int[] { prevchd.Value }; else PrevCHD.ClearObservedValue();
            if (prevmi.HasValue) PrevMI.ObservedValue = new int[] { prevmi.Value }; else PrevMI.ClearObservedValue();
            if (prevstrk.HasValue) PrevStrk.ObservedValue = new int[] { prevstrk.Value }; else PrevStrk.ClearObservedValue();
            if (prevhyp.HasValue) PrevHyp.ObservedValue = new int[] { prevhyp.Value }; else PrevHyp.ClearObservedValue();
            if (age.HasValue) Age.ObservedValue = new int[] { age.Value }; else Age.ClearObservedValue();
            if (bmi.HasValue) BMI.ObservedValue = new int[] { bmi.Value }; else BMI.ClearObservedValue();
            if (cursmoke.HasValue) CurSmoke.ObservedValue = new int[] { cursmoke.Value }; else CurSmoke.ClearObservedValue();
            if (hdlc.HasValue) HDLC.ObservedValue = new int[] { hdlc.Value }; else HDLC.ClearObservedValue();
            if (diabetes.HasValue) Diabetes.ObservedValue = new int[] { diabetes.Value }; else Diabetes.ClearObservedValue();
            if (glucose.HasValue) Glucose.ObservedValue = new int[] { glucose.Value }; else Glucose.ClearObservedValue();
            if (cvd.HasValue && mode!=(int)modes.cvd) CVD.ObservedValue = new int[] { cvd.Value }; else CVD.ClearObservedValue();
            if (sysbp.HasValue) SysBP.ObservedValue = new int[] { sysbp.Value }; else SysBP.ClearObservedValue();
            if (diabp.HasValue) DiaBP.ObservedValue = new int[] { diabp.Value }; else DiaBP.ClearObservedValue();
            if (hyperten.HasValue && mode!=(int)modes.hyperten) Hyperten.ObservedValue = new int[] { hyperten.Value }; else Hyperten.ClearObservedValue();
            if (ldlc.HasValue) LDLC.ObservedValue = new int[] { ldlc.Value }; else LDLC.ClearObservedValue();
            if (totchol.HasValue) TotChol.ObservedValue = new int[] { totchol.Value }; else TotChol.ClearObservedValue();
            if (angina.HasValue && mode != (int)modes.angina) Angina.ObservedValue = new int[] { angina.Value }; else Angina.ClearObservedValue();
            if (mi_fchd.HasValue && mode != (int)modes.mi_fdch) MI_FCHD.ObservedValue = new int[] { mi_fchd.Value }; else MI_FCHD.ClearObservedValue();
            if (hospmi.HasValue && mode != (int)modes.hospmi) HospMI.ObservedValue = new int[] { hospmi.Value }; else HospMI.ClearObservedValue();
            if (anychd.HasValue && mode != (int)modes.anychd) AnyCHD.ObservedValue = new int[] { anychd.Value }; else AnyCHD.ClearObservedValue();
            if (stroke.HasValue && mode != (int)modes.stroke) Stroke.ObservedValue = new int[] { stroke.Value }; else Stroke.ClearObservedValue();
            if (death.HasValue && mode != (int)modes.death) Death.ObservedValue = new int[] { death.Value }; else Death.ClearObservedValue();

            #endregion

            #region SetProb&CPTPriorObservedValue

            ProbSexPrior.ObservedValue = probSexPrior;
            ProbBPMedsPrior.ObservedValue = probBPMedsPrior;
            ProbPrevAPPrior.ObservedValue = probPrevAPPrior;
            ProbPrevCHDPrior.ObservedValue = probPrevCHDPrior;
            ProbPrevMIPrior.ObservedValue = probPrevMIPrior;
            ProbPrevStrkPrior.ObservedValue = probPrevStrkPrior;
            ProbPrevHypPrior.ObservedValue = probPrevHypPrior;

            CPTAgePrior.ObservedValue = cptAgePrior;
            CPTBMIPrior.ObservedValue = cptBMIPrior;
            CPTCurSmokePrior.ObservedValue = cptCurSmokePrior;
            CPTHDLCPrior.ObservedValue = cptHDLCPrior;
            CPTDiabetesPrior.ObservedValue = cptDiabetesPrior;
            CPTGlucosePrior.ObservedValue = cptGlucosePrior;
            CPTCVDPrior.ObservedValue = cptCVDPrior;
            CPTSysBPPrior.ObservedValue = cptSysBPPrior;
            CPTDiaBPPrior.ObservedValue = cptDiaBPPrior;
            CPTHypertenPrior.ObservedValue = cptHypertenPrior;
            CPTLDLCPrior.ObservedValue = cptLDLCPrior;
            CPTTotCholPrior.ObservedValue = cptTotCholPrior;
            CPTAnginaPrior.ObservedValue = cptAnginaPrior;
            CPTMI_FCHDPrior.ObservedValue = cptMI_FCHDPrior;
            CPTHospMIPrior.ObservedValue = cptHospMIPrior;
            CPTAnyCHDPrior.ObservedValue = cptAnyCHDPrior;
            CPTStrokePrior.ObservedValue = cptStrokePrior;
            CPTDeathPrior.ObservedValue = cptDeathPrior;
            
            #endregion

            // Inference
            switch(mode)
            {
                case 1:
                    var anginaPosterior = Engine.Infer<Discrete[]>(Angina);
                    return anginaPosterior[0].GetProbs()[0];
                case 2:
                   var mi_fchdPosterior = Engine.Infer<Discrete[]>(MI_FCHD);
                    return mi_fchdPosterior[0].GetProbs()[0];
                case 3:
                    var hospmiPosterior = Engine.Infer<Discrete[]>(HospMI);
                    return hospmiPosterior[0].GetProbs()[0];
                case 4:
                    var anychdPosterior = Engine.Infer<Discrete[]>(AnyCHD);
                    return anychdPosterior[0].GetProbs()[0];
                case 5:
                  var strokePosterior = Engine.Infer<Discrete[]>(Stroke);
                    return strokePosterior[0].GetProbs()[0];
                case 6:
                    var cvdPosterior = Engine.Infer<Discrete[]>(CVD);
                    return cvdPosterior[0].GetProbs()[0];
                case 7:
                    var hypertenPosterior = Engine.Infer<Discrete[]>(Hyperten);
                    return hypertenPosterior[0].GetProbs()[0];
                case 8:
                    var deathPosterior = Engine.Infer<Discrete[]>(Death);
                    return deathPosterior[0].GetProbs()[0];
                default:
                    throw new Exception();
            }                  
        }

        public double ProbDisease(int mode,
            int? sex, int? age, int? sysbp, int? diabp, int? bpmeds, int? cursmoke, int? totchol, int? hdlc, int? ldlc,
            int? bmi, int? glucose, int? diabetes, int? prevap, int? prevchd, int? prevmi, int? prevstrk, int? prevhyp,
            int? angina, int? hospmi, int? mi_fchd, int? anychd, int? stroke, int? cvd, int? hyperten, int? death,
            Vector probSex, Vector probBPMeds, Vector probPrevAP, Vector probPrevCHD, Vector probPrevMI,Vector probPrevStrk, Vector probPrevHyp,
            Vector[] cptAge, Vector[] cptHDLC, Vector[] cptGlucose, Vector[] cptDiabetes, Vector[][] cptBMI, Vector[][] cptCurSmoke,
            Vector[][][] cptDiaBP, Vector[][][] cptCVD, Vector[][][] cptHyperten, Vector[][][][] cptSysBP, Vector[][][][] cptTotChol, Vector[][][][] cptLDLC,
            Vector[][][][][] cptStroke, Vector[][][][][] cptDeath, Vector[][][][][][] cptAngina, Vector[][][][][][] cptAnyCHD,
            Vector[][][][][][][][] cptMI_FCHD, Vector[][][][][][][][] cptHospMI
            )
        {
            var probSexPrior = Dirichlet.PointMass(probSex);
            var probBPMedsPrior = Dirichlet.PointMass(probBPMeds);
            var probPrevAPPrior = Dirichlet.PointMass(probPrevAP);
            var probPrevCHDPrior = Dirichlet.PointMass(probPrevCHD);
            var probPrevMIPrior = Dirichlet.PointMass(probPrevMI);
            var probPrevStrkPrior = Dirichlet.PointMass(probPrevStrk);
            var probPrevHypPrior = Dirichlet.PointMass(probPrevHyp);
            var cptAgePrior = cptAge.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptHDLCPrior = cptHDLC.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptGlucosePrior = cptGlucose.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptDiabetesPrior = cptDiabetes.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptBMIPrior = cptBMI.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptCurSmokePrior = cptCurSmoke.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptDiaBPPrior = cptDiaBP.Select(v3=>v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptCVDPrior = cptCVD.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptHypertenPrior = cptHyperten.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptSysBPPrior = cptSysBP.Select(v4=>v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptTotCholPrior = cptTotChol.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptLDLCPrior = cptLDLC.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptStrokePrior = cptStroke.Select(v5=>v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptDeathPrior = cptDeath.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptAnginaPrior = cptAngina.Select(v6=>v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptAnyCHDPrior = cptAnyCHD.Select(v6 => v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptMI_FCHDPrior = cptMI_FCHD.Select(v8=>v8.Select(v7=>v7.Select(v6 => v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
           var cptHospMIPrior=cptHospMI.Select(v8=>v8.Select(v7=>v7.Select(v6 => v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            return ProbDisease(mode, 
                sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, glucose, diabetes, prevap, prevchd,
                prevmi, prevstrk, prevhyp, angina, hospmi, mi_fchd, anychd, stroke, cvd, hyperten, death,
                probSexPrior, probBPMedsPrior, probPrevAPPrior, probPrevCHDPrior, probPrevMIPrior, probPrevStrkPrior, probPrevHypPrior,
                cptAgePrior, cptHDLCPrior, cptGlucosePrior, cptDiabetesPrior, cptBMIPrior, cptCurSmokePrior, cptDiaBPPrior,
                cptCVDPrior, cptHypertenPrior, cptSysBPPrior, cptTotCholPrior, cptLDLCPrior, cptStrokePrior, cptDeathPrior,
                cptAnginaPrior, cptAnyCHDPrior, cptMI_FCHDPrior, cptHospMIPrior);
        }

        /// Sample the model
        public static int[][] Sample( int numData,
            Vector probSex, Vector probBPMeds, Vector probPrevAP, Vector probPrevCHD, Vector probPrevMI, Vector probPrevStrk, Vector probPrevHyp, 
            Vector[] cptAge, Vector[] cptHDLC, Vector[] cptGlucose, Vector[] cptDiabetes, Vector[][] cptBMI, 
            Vector[][] cptCurSmoke, Vector[][][] cptDiaBP, Vector[][][] cptCVD, Vector[][][] cptHyperten,
            Vector[][][][] cptSysBP, Vector[][][][] cptTotChol, Vector[][][][] cptLDLC, Vector[][][][][] cptStroke,
            Vector[][][][][] cptDeath, Vector[][][][][][] cptAngina, Vector[][][][][][] cptAnyCHD, Vector[][][][][][][][] cptMI_FCHD,
            Vector[][][][][][][][] cptHospMI
            )
        {
            int[][] sample = new int[varCount][];
            for (int i = 0; i < varCount; i++) sample[i] = new int[numData];
            for (int i = 0; i < numData; i++)
            {
                int sex=Discrete.Sample(probSex);
                int bpmeds=Discrete.Sample(probBPMeds);
                int prevap=Discrete.Sample(probPrevAP);
                int prevchd=Discrete.Sample(probPrevCHD);
                int prevmi=Discrete.Sample(probPrevMI);
                int prevstrk=Discrete.Sample(probPrevStrk);
                int prevhyp=Discrete.Sample(probPrevHyp);

                int age=Discrete.Sample(cptAge[sex]);
                int bmi=Discrete.Sample(cptBMI[sex][age]);
                int cursmoke=Discrete.Sample(cptCurSmoke[sex][age]);
                int hdlc=Discrete.Sample(cptHDLC[bmi]);
                int diabetes=Discrete.Sample(cptDiabetes[bmi]);
                int glucose=Discrete.Sample(cptGlucose[diabetes]);
                int cvd=Discrete.Sample(cptCVD[prevchd][cursmoke][prevmi]);
                int sysbp=Discrete.Sample(cptSysBP[bmi][age][cursmoke][bpmeds]);
                int diabp=Discrete.Sample(cptDiaBP[sysbp][age][cvd]);
                int hyperten=Discrete.Sample(cptHyperten[age][prevhyp][diabp]);
                int ldlc=Discrete.Sample(cptLDLC[bmi][diabetes][cursmoke][hyperten]);
                int totchol=Discrete.Sample(cptTotChol[sex][age][hdlc][ldlc]);
                int angina=Discrete.Sample(cptAngina[prevap][diabetes][diabp][cursmoke][sex][age]);
                int mi_fchd=Discrete.Sample(cptMI_FCHD[prevmi][prevchd][diabetes][diabp][totchol][cvd][sex][age]);
                int hospmi=Discrete.Sample(cptHospMI[prevmi][diabetes][diabp][totchol][angina][sex][age][mi_fchd]);
                int anychd=Discrete.Sample(cptAnyCHD[prevchd][hospmi][angina][diabp][cvd][diabetes]);
                int stroke=Discrete.Sample(cptStroke[prevstrk][anychd][diabp][cursmoke][diabetes]);
                int death=Discrete.Sample(cptDeath[age][hospmi][mi_fchd][stroke][anychd]);

                sample[0][i] = sex;
                sample[1][i] = age;
                sample[2][i] = sysbp;
                sample[3][i] = diabp;
                sample[4][i] = bpmeds;
                sample[5][i] = cursmoke;
                sample[6][i] = totchol;
                sample[7][i] = hdlc;
                sample[8][i] = ldlc;
                sample[9][i] = bmi;
                sample[10][i] = glucose;
                sample[11][i] = diabetes;
                sample[12][i] = prevap;
                sample[13][i] = prevchd;
                sample[14][i] = prevmi;
                sample[15][i] = prevstrk;
                sample[16][i] = prevhyp;
                sample[17][i] = angina;
                sample[18][i] = hospmi;
                sample[19][i] = mi_fchd;
                sample[20][i] = anychd;
                sample[21][i] = stroke;
                sample[22][i] = cvd;
                sample[23][i] = hyperten;
                sample[24][i] = death;
            }
            return sample;
            
        }

        //Add child from n parents
        public static VariableArray<int> AddChildFromOneParent(
            VariableArray<int> parent,
            VariableArray<Vector> cpt)
        {
            var n = parent.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent[n]))
                child[n] = Variable.Discrete(cpt[parent[n]]);
            return child;
        }

        public static VariableArray<int> AddChildFromTwoParents(
            VariableArray<int> parent1, VariableArray<int> parent2,
            VarVectArr2 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
                child[n] = Variable.Discrete(cpt[parent1[n]][parent2[n]]);
            return child;
        }

        public static VariableArray<int> AddChildFromThreeParents(
            VariableArray<int> parent1, VariableArray<int> parent2, VariableArray<int> parent3,
            VarVectArr3 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
            using (Variable.Switch(parent3[n]))
                child[n] = Variable.Discrete(cpt[parent1[n]][parent2[n]][parent3[n]]);
            return child;
        }

        public static VariableArray<int> AddChildFromFourParents(
            VariableArray<int> parent1, VariableArray<int> parent2, VariableArray<int> parent3, VariableArray<int> parent4,
            VarVectArr4 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
            using (Variable.Switch(parent3[n]))
            using (Variable.Switch(parent4[n]))
                child[n] = Variable.Discrete(
                    cpt[parent1[n]][parent2[n]][parent3[n]][parent4[n]]);
            return child;
        }

        public static VariableArray<int> AddChildFromFiveParents(
            VariableArray<int> parent1, VariableArray<int> parent2, VariableArray<int> parent3,
            VariableArray<int> parent4, VariableArray<int> parent5,
            VarVectArr5 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
            using (Variable.Switch(parent3[n]))
            using (Variable.Switch(parent4[n]))
            using (Variable.Switch(parent5[n]))
                child[n] = Variable.Discrete(
                    cpt[parent1[n]][parent2[n]][parent3[n]][parent4[n]][parent5[n]]);
            return child;
        }

        public static VariableArray<int> AddChildFromSixParents(
            VariableArray<int> parent1, VariableArray<int> parent2, VariableArray<int> parent3,
            VariableArray<int> parent4, VariableArray<int> parent5, VariableArray<int> parent6,
            VarVectArr6 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
            using (Variable.Switch(parent3[n]))
            using (Variable.Switch(parent4[n]))
            using (Variable.Switch(parent5[n]))
            using (Variable.Switch(parent6[n]))
                child[n] = Variable.Discrete(
                    cpt[parent1[n]][parent2[n]][parent3[n]][parent4[n]][parent5[n]][parent6[n]]);
            return child;
        }
        public static VariableArray<int> AddChildFromEightParents(
            VariableArray<int> parent1, VariableArray<int> parent2, VariableArray<int> parent3, VariableArray<int> parent4,
            VariableArray<int> parent5, VariableArray<int> parent6, VariableArray<int> parent7, VariableArray<int> parent8,
            VarVectArr8 cpt)
        {
            var n = parent1.Range;
            var child = Variable.Array<int>(n);
            using (Variable.ForEach(n))
            using (Variable.Switch(parent1[n]))
            using (Variable.Switch(parent2[n]))
            using (Variable.Switch(parent3[n]))
            using (Variable.Switch(parent4[n]))
            using (Variable.Switch(parent5[n]))
            using (Variable.Switch(parent6[n]))
            using (Variable.Switch(parent7[n]))
            using (Variable.Switch(parent8[n]))
                child[n] = Variable.Discrete(
                    cpt[parent1[n]][parent2[n]][parent3[n]][parent4[n]][parent5[n]][parent6[n]][parent7[n]][parent8[n]]);
            return child;
        }
    }
}

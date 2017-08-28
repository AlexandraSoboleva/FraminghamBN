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
       //const int varCount=25;
      
        #region netVariables
        //Primary random variables
        //All variables have several states (if variable is continuos, ranges it's values)
        //Characteristics or risk factors
       // public VariableArray<int> RandID;    //Unique identification number for each participant     2448-9999312
        public VariableArray<int> Sex;      //Participant sex    1=Men, 2=Women 
        public VariableArray<int> Periodd;   //Examination Cycle   1=Period 1, 2=Period 2, 3=Period 3 
        public VariableArray<int> Time;     //Number of days since baseline exam    0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
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
        
        public VariableArray<int> TimeAP;   //Number of days from Baseline exam to first Angina during the followup or Number of days from Baseline to censor date. Censor date may be end of followup, death or last known contact date if subject is lost to followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeMI;   //Defined as above for the first HOSPMI event during followup
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeMIFC; //Defined as above for the first MI_FCHD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeCHD;  //Defined as above for the first ANYCHD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeStrk; //Defined as above for the first STROKE event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeCVD;  //Defined as above for the first CVD event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeHyp;  //Defined as above for the first HYPERTEN event during followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        public VariableArray<int> TimeDth;  //Number of days from Baseline exam to death if occurring during followup or Number of days from Baseline to censor date. Censor date may be end of followup, or last known contact date if subject is lost to followup 
                                            //0-4854: 1=0-1618, 2=1619-3236, 3=3237-4854
        
        #endregion
        public Variable<int> NumberOfExamples;

        #region Prob&CPT
        // Random variables representing the parameters of the distributions
        // of the primary random variables. For child variables, these are
        // in the form of conditional probability tables (CPTs)
        //None parent
        //public Variable<Vector> ProbRandID;
        public Variable<Vector> ProbSex;  
        public Variable<Vector> ProbPeriodd;
        public Variable<Vector> ProbBPMeds;
       
        //One parent
              //parent: RandID
        public VariableArray<Vector> CPTTime;       //parent: Period
        public VariableArray<Vector> CPTHDLC;       //parent: BMI
        public VariableArray<Vector> CPTGlucose;    //parent: Diabetes
        public VariableArray<Vector> CPTDiabetes;   //parent: BMI   
        public VariableArray<Vector> CPTCVD;        //parents: CurSmoke
        //Two parents
        public VarVectArr2 CPTAge;        //parent: Sex, Period
        public VarVectArr2 CPTBMI;         //parents: Sex, Age
        public VarVectArr2 CPTCurSmoke;    //parents: Sex, Age
        public VarVectArr2 CPTTimeAP;     //parent: Time, Angina
        public VarVectArr2 CPTTimeCVD;    //parent: Time, CVD 
        public VarVectArr2 CPTTimeHyp;    //parent: Time, Hyperten 
        public VarVectArr2 CPTHyperten;    //parents: Age, DiaBP

        public VarVectArr2 CPTPrevAP;      //parent: Angina, TimaAP
        public VarVectArr2 CPTPrevCHD;     //parent: Any_CHD, TimaCHD
        public VarVectArr2 CPTPrevMI;      //parent: HospMI, TimaMI
        public VarVectArr2 CPTPrevStrk;    //parent: Stroke, TimaStrk
        public VarVectArr2 CPTPrevHyp;     //parent: Hyperten, TimaHyp
        //Three parents
        public VarVectArr3 CPTDiaBP;       //parents: SysBP, Age, CVD
        public VarVectArr3 CPTTimeMIFC;   //parent: Time, MI_FCHD, TimeCVD
        public VarVectArr3 CPTTimeSTRK;   //parent: Time, Stroke, TimeCHD 
        //Four parents
        public VarVectArr4 CPTSysBP;       //parents: BMI, Age, CurSmoke, BPMeds
        public VarVectArr4 CPTTotChol;     //parents: Sex, Age, HDLC, LDLC
        public VarVectArr4 CPTLDLC;        //parents: BMI, Diabetes, CurSmoke, Hyperten
        public VarVectArr4 CPTTimeMI;     //parent: Time, MI, TimeAP, TimeMIFC
        public VarVectArr4 CPTStroke;      //parents: AnyCHD, DiaBP, CurSmoke, Diabetes
        //Five parents
        public VarVectArr5 CPTDeath;       //parents: Age, HospMI, MI_FCHD, Stroke, AnyCHD
        public VarVectArr5 CPTTimeCHD;    //parent: Time, Any_CHD, TimeMI, TimeAP, TimeCVD 
        public VarVectArr5 CPTTimeDth;     //parents: Death, TimeMI, TimeMIFC, TimeCHD, TimeStrk
        public VarVectArr5 CPTAngina;      //parents: Diabetes, DiaBP, CurSmoke, Sex, Age
        public VarVectArr5 CPTAnyCHD;     //parents: HospMI, Angina, DiaBP, CVD, Diabetes
        //Six parants
        public VarVectArr6 CPTMI_FCHD;     //parents: Diabetes, DiaBP, TotChol, CVD, Sex, Age
        //Eight parents
        public VarVectArr8 CPTHospMI;      //parents: CVD, Diabetes, DiaBP, TotChol, Angina, Sex, Age, MI_FCHD
        #endregion

        #region PriorProb&CPT
        // Prior distributions for the probability and CPT variables.
        // The prior distributions are formulated as Infer.NET variables
        // so that they can be set at runtime without recompiling the model
        
        //public Variable<Dirichlet> ProbRandIDPrior;
        public Variable<Dirichlet> ProbSexPrior;
        public Variable<Dirichlet> ProbPerioddPrior;
        public Variable<Dirichlet> ProbBPMedsPrior;
        public VariableArray<Dirichlet> CPTTimePrior;
        public VariableArray<Dirichlet> CPTHDLCPrior;
        public VariableArray<Dirichlet> CPTGlucosePrior;
        public VariableArray<Dirichlet> CPTDiabetesPrior;
        public VarDirArr2 CPTAgePrior;
        public VarDirArr2 CPTBMIPrior;
        public VarDirArr2 CPTCurSmokePrior;
        public VarDirArr2 CPTTimeAPPrior;  
        public VarDirArr4 CPTTimeMIPrior;  
        public VarDirArr3 CPTTimeMIFCPrior;
        public VarDirArr5 CPTTimeCHDPrior; 
        public VarDirArr3 CPTTimeSTRKPrior;
        public VarDirArr2 CPTTimeCVDPrior; 
        public VarDirArr2 CPTTimeHypPrior; 
        public VarDirArr3 CPTDiaBPPrior;
        public VariableArray<Dirichlet> CPTCVDPrior;
        public VarDirArr2 CPTHypertenPrior;
        public VarDirArr4 CPTSysBPPrior;
        public VarDirArr4 CPTTotCholPrior;
        public VarDirArr4 CPTLDLCPrior;
        public VarDirArr4 CPTStrokePrior;
        public VarDirArr5 CPTDeathPrior;
        public VarDirArr5 CPTAnginaPrior;
        public VarDirArr5 CPTAnyCHDPrior;
        public VarDirArr6 CPTMI_FCHDPrior;
        public VarDirArr8 CPTHospMIPrior;
        public VarDirArr2 CPTPrevAPPrior;
        public VarDirArr2 CPTPrevCHDPrior;
        public VarDirArr2 CPTPrevMIPrior;
        public VarDirArr2 CPTPrevStrkPrior;
        public VarDirArr2 CPTPrevHypPrior;
        public VarDirArr5 CPTTimeDthPrior;

        #endregion

        #region PosteriorProb&CPT
        // Posterior distributions for the probability and CPT variables
      //  public Dirichlet ProbRandIDPosterior;
        public Dirichlet ProbSexPosterior;
        public Dirichlet ProbPerioddPosterior;
        public Dirichlet ProbBPMedsPosterior;
        public Dirichlet[][] CPTPrevAPPosterior;
        public Dirichlet[][] CPTPrevCHDPosterior;
        public Dirichlet[][] CPTPrevMIPosterior;
        public Dirichlet[][] CPTPrevStrkPosterior;
        public Dirichlet[][] CPTPrevHypPosterior;
        public Dirichlet[] CPTTimePosterior;
        public Dirichlet[] CPTHDLCPosterior;
        public Dirichlet[] CPTGlucosePosterior;
        public Dirichlet[] CPTDiabetesPosterior;
        public Dirichlet[][] CPTAgePosterior;
        public Dirichlet[][] CPTBMIPosterior;
        public Dirichlet[][] CPTCurSmokePosterior;
        public Dirichlet[][] CPTTimeAPPosterior;
        public Dirichlet[][][][] CPTTimeMIPosterior;
        public Dirichlet[][][] CPTTimeMIFCPosterior;
        public Dirichlet[][][][][] CPTTimeCHDPosterior;
        public Dirichlet[][][] CPTTimeSTRKPosterior;
        public Dirichlet[][] CPTTimeCVDPosterior;
        public Dirichlet[][] CPTTimeHypPosterior; 
        public Dirichlet[][][] CPTDiaBPPosterior;
        public Dirichlet[] CPTCVDPosterior;
        public Dirichlet[][] CPTHypertenPosterior;
        public Dirichlet[][][][] CPTSysBPPosterior;
        public Dirichlet[][][][] CPTTotCholPosterior;
        public Dirichlet[][][][] CPTLDLCPosterior;
        public Dirichlet[][][][] CPTStrokePosterior;
        public Dirichlet[][][][][] CPTDeathPosterior;
        public Dirichlet[][][][][] CPTAnginaPosterior;
        public Dirichlet[][][][][] CPTAnyCHDPosterior;
        public Dirichlet[][][][][][] CPTMI_FCHDPosterior;
        public Dirichlet[][][][][][][][] CPTHospMIPosterior;
        public Dirichlet[][][][][] CPTTimeDthPosterior;
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
           // Range RRandID = new Range(9996864).Named("RRandID");
            Range RPeriod = new Range(3).Named("RPeriod");
            Range RTime = new Range(3).Named("RTime");
            Range RTimeAP = new Range(6).Named("RTimeAP");
            Range RTimeMI = new Range(6).Named("RTimeMI");
            Range RTimeMIFC = new Range(6).Named("RTimeMIFC");
            Range RTimeCHD = new Range(6).Named("RTimeCHD");
            Range RTimeStrk = new Range(6).Named("RTimeStrk");
            Range RTimeCVD = new Range(6).Named("RTimeCVD");
            Range RTimeHyp = new Range(6).Named("RTimeHyp");
            Range RTimeDth = new Range(6).Named("RTimeDth");
            #endregion

            #region DefinePriors
            // Define the priors and the parameters
           /* ProbRandIDPrior = Variable.New<Dirichlet>().Named("ProbRandIDPrior");
            ProbRandID = Variable<Vector>.Random(ProbRandIDPrior).Named("ProbRandID");
            ProbRandID.SetValueRange(RRandID);*/

            ProbSexPrior = Variable.New<Dirichlet>().Named("ProbSexPrior");
            ProbSex = Variable<Vector>.Random(ProbSexPrior).Named("ProbSex");
            ProbSex.SetValueRange(RSex);

            ProbPerioddPrior = Variable.New<Dirichlet>().Named("ProbPeriodPrior");
            ProbPeriodd = Variable<Vector>.Random(ProbPerioddPrior).Named("ProbPeriod");
            ProbPeriodd.SetValueRange(RPeriod);

            ProbBPMedsPrior = Variable.New<Dirichlet>().Named("ProbBPMedsPrior");
            ProbBPMeds = Variable<Vector>.Random(ProbBPMedsPrior).Named("ProbBPMeds");
            ProbBPMeds.SetValueRange(RBPMeds);

            CPTTimePrior = Variable.Array<Dirichlet>(RPeriod).Named("CPTTimePrior");
            CPTTime = Variable.Array<Vector>(RPeriod).Named("CPTTime");
            CPTTime[RPeriod] = Variable<Vector>.Random(CPTTimePrior[RPeriod]);
            CPTTime.SetValueRange(RTime);

            CPTAgePrior = Variable.Array(Variable.Array<Dirichlet>(RSex),RPeriod).Named("CPTAgePrior");
            CPTAge = Variable.Array(Variable.Array<Vector>(RSex),RPeriod).Named("CPTAge");
            CPTAge[RPeriod][RSex] = Variable<Vector>.Random(CPTAgePrior[RPeriod][RSex]);
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
            
            CPTCVDPrior =Variable.Array<Dirichlet>( RCurSmoke).Named("CPTCVDPrior");
            CPTCVD = Variable.Array<Vector>( RCurSmoke).Named("CPTCVD");
            CPTCVD[RCurSmoke]= Variable<Vector>.Random(CPTCVDPrior[RCurSmoke]);
            CPTCVD.SetValueRange(RCVD);

            CPTSysBPPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RBMI), RCurSmoke), RBPMeds).Named("CPTSysBPPrior");
            CPTSysBP = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RBMI), RCurSmoke), RBPMeds).Named("CPTSysBP");
            CPTSysBP[RBPMeds][RCurSmoke][RBMI][RAge] = Variable<Vector>.Random(CPTSysBPPrior[RBPMeds][RCurSmoke][RBMI][RAge]);
            CPTSysBP.SetValueRange(RSysBP);

            CPTDiaBPPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RSysBP), RAge), RCVD).Named("CPTDiaBPPrior");
            CPTDiaBP = Variable.Array(Variable.Array(Variable.Array<Vector>(RSysBP), RAge), RCVD).Named("CPTDiaBP");
            CPTDiaBP[RCVD][RAge][RSysBP] = Variable<Vector>.Random(CPTDiaBPPrior[RCVD][RAge][RSysBP]);
            CPTDiaBP.SetValueRange(RDiaBP);

            CPTHypertenPrior = Variable.Array(Variable.Array<Dirichlet>(RAge), RDiaBP).Named("CPTHypertenPrior");
            CPTHyperten = Variable.Array(Variable.Array<Vector>(RAge), RDiaBP).Named("CPTHyperten");
            CPTHyperten[RDiaBP][RAge] = Variable<Vector>.Random(CPTHypertenPrior[RDiaBP][RAge]);
            CPTHyperten.SetValueRange(RHyperten);

            CPTLDLCPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RDiabetes), RBMI), RCurSmoke), RHyperten).Named("CPTLDLCPrior");
            CPTLDLC = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RDiabetes), RBMI), RCurSmoke), RHyperten).Named("CPTLDLC");
            CPTLDLC[RHyperten][RCurSmoke][RBMI][RDiabetes] = Variable<Vector>.Random(CPTLDLCPrior[RHyperten][RCurSmoke][RBMI][RDiabetes]);
            CPTLDLC.SetValueRange(RLDLC);

            CPTTotCholPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RSex), RHDLC), RLDLC).Named("CPTTotCholPrior");
            CPTTotChol = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RSex), RHDLC), RLDLC).Named("CPTTotChol");
            CPTTotChol[RLDLC][RHDLC][RSex][RAge] = Variable<Vector>.Random(CPTTotCholPrior[RLDLC][RHDLC][RSex][RAge]);
            CPTTotChol.SetValueRange(RTotChol);

            CPTAnginaPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RDiabetes), RDiaBP), RCurSmoke), RSex).Named("CPTAnginaPrior");
            CPTAngina = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RDiabetes), RDiaBP), RCurSmoke), RSex).Named("CPTAngina");
            CPTAngina[RSex][RCurSmoke][RDiaBP][RDiabetes][RAge] = Variable<Vector>.Random(CPTAnginaPrior[RSex][RCurSmoke][RDiaBP][RDiabetes][RAge]);
            CPTAngina.SetValueRange(RAngina);

            CPTMI_FCHDPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>( RDiabetes), RDiaBP), RTotChol), RCVD), RSex), RAge).Named("CPTMI_FCHDPrior");
            CPTMI_FCHD = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>( RDiabetes), RDiaBP), RTotChol), RCVD), RSex), RAge).Named("CPTMI_FCHD");
            CPTMI_FCHD[RAge][RSex][RCVD][RTotChol][RDiaBP][RDiabetes] = Variable<Vector>.Random(CPTMI_FCHDPrior[RAge][RSex][RCVD][RTotChol][RDiaBP][RDiabetes]);
            CPTMI_FCHD.SetValueRange(RMI_FCHD);

            CPTHospMIPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAngina), RDiabetes), RDiaBP), RCVD), RTotChol), RMI_FCHD), RSex), RAge).Named("CPTHospMIPrior");
            CPTHospMI = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAngina), RDiabetes), RDiaBP), RCVD), RTotChol), RMI_FCHD), RSex), RAge).Named("CPTHospMI");
            CPTHospMI[RAge][RSex][RMI_FCHD][RTotChol][RCVD][RDiaBP][RDiabetes][RAngina] = Variable<Vector>.Random(CPTHospMIPrior[RAge][RSex][RMI_FCHD][RTotChol][RCVD][RDiaBP][RDiabetes][RAngina]);
            CPTHospMI.SetValueRange(RHospMI);


            CPTAnyCHDPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>( RDiabetes), RDiaBP), RHospMI), RAngina), RCVD).Named("CPTAnyCHDPrior");
            CPTAnyCHD = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>( RDiabetes), RDiaBP), RHospMI), RAngina), RCVD).Named("CPTAnyCHD");
            CPTAnyCHD[RCVD][RAngina][RHospMI][RDiaBP][RDiabetes] = Variable<Vector>.Random(CPTAnyCHDPrior[RCVD][RAngina][RHospMI][RDiaBP][RDiabetes]);
            CPTAnyCHD.SetValueRange(RAnyCHD);

            CPTStrokePrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>( RAnyCHD), RCurSmoke), RDiaBP), RDiabetes).Named("CPTStrokePrior");
            CPTStroke = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>( RAnyCHD), RCurSmoke), RDiaBP), RDiabetes).Named("CPTStroke");
            CPTStroke[RDiabetes][RDiaBP][RCurSmoke][RAnyCHD] = Variable<Vector>.Random(CPTStrokePrior[RDiabetes][RDiaBP][RCurSmoke][RAnyCHD]);
            CPTStroke.SetValueRange(RStroke);

            CPTTimeAPPrior = Variable.Array(Variable.Array<Dirichlet>(RTime), RAngina).Named("CPTTimeAPPrior");
            CPTTimeAP = Variable.Array(Variable.Array<Vector>(RTime), RAngina).Named("CPTTimeAP");
            CPTTimeAP[RAngina][RTime] = Variable<Vector>.Random(CPTTimeAPPrior[RAngina][RTime]);
            CPTTimeAP.SetValueRange(RTimeAP);

            CPTTimeCVDPrior = Variable.Array(Variable.Array<Dirichlet>(RTime), RCVD).Named("CPTTimeCVDPrior");
            CPTTimeCVD = Variable.Array(Variable.Array<Vector>(RTime), RCVD).Named("CPTTimeCVD");
            CPTTimeCVD[RCVD][RTime] = Variable<Vector>.Random(CPTTimeCVDPrior[RCVD][RTime]);
            CPTTimeCVD.SetValueRange(RTimeCVD);

            CPTTimeHypPrior = Variable.Array(Variable.Array<Dirichlet>(RTime), RHyperten).Named("CPTTimeHypPrior");
            CPTTimeHyp = Variable.Array(Variable.Array<Vector>(RTime), RHyperten).Named("CPTTimeHyp");
            CPTTimeHyp[RHyperten][RTime] = Variable<Vector>.Random(CPTTimeHypPrior[RHyperten][RTime]);
            CPTTimeHyp.SetValueRange(RTimeHyp);

            CPTTimeMIFCPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RTime), RMI_FCHD), RTimeCVD).Named("CPTTimeMIFCPrior");
            CPTTimeMIFC = Variable.Array(Variable.Array(Variable.Array<Vector>(RTime), RMI_FCHD),RTimeCVD).Named("CPTTimeMIFC");
            CPTTimeMIFC[RTimeCVD][RMI_FCHD][RTime] = Variable<Vector>.Random(CPTTimeMIFCPrior[RTimeCVD][RMI_FCHD][RTime]);
            CPTTimeMIFC.SetValueRange(RTimeMIFC);

            CPTTimeMIPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RTime), RHospMI),RTimeAP),RTimeMIFC).Named("CPTTimeMIPrior");
            CPTTimeMI = Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RTime), RHospMI), RTimeAP), RTimeMIFC).Named("CPTTimeMI");
            CPTTimeMI[RTimeMIFC][RTimeAP][RHospMI][RTime] = Variable<Vector>.Random(CPTTimeMIPrior[RTimeMIFC][RTimeAP][RHospMI][RTime]);
            CPTTimeMI.SetValueRange(RTimeMI);

            CPTTimeCHDPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RTime), RAnyCHD),RTimeMI), RTimeAP), RTimeCVD).Named("CPTTimeCHDPrior");
            CPTTimeCHD = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RTime), RAnyCHD), RTimeMI), RTimeAP), RTimeCVD).Named("CPTTimeCHD");
            CPTTimeCHD[RTimeCVD][RTimeAP][RTimeMI][RAnyCHD][RTime] = Variable<Vector>.Random(CPTTimeCHDPrior[RTimeCVD][RTimeAP][RTimeMI][RAnyCHD][RTime]);
            CPTTimeCHD.SetValueRange(RTimeCHD);

            CPTTimeSTRKPrior = Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RTime), RStroke),RTimeCHD).Named("CPTTimeSTRKPrior");
            CPTTimeSTRK = Variable.Array(Variable.Array(Variable.Array<Vector>(RTime), RStroke), RTimeCHD).Named("CPTTimeSTRK");
            CPTTimeSTRK[RTimeCHD][RStroke][RTime] = Variable<Vector>.Random(CPTTimeSTRKPrior[RTimeCHD][RStroke][RTime]);
            CPTTimeSTRK.SetValueRange(RTimeStrk);

            CPTPrevAPPrior = Variable.Array(Variable.Array<Dirichlet>(RAngina), RTimeAP).Named("CPTPrevAPPrior");
            CPTPrevAP = Variable.Array(Variable.Array<Vector>(RAngina), RTimeAP).Named("CPTPrevAP");
            CPTPrevAP[RTimeAP][RAngina] = Variable<Vector>.Random(CPTPrevAPPrior[RTimeAP][RAngina]);
            CPTPrevAP.SetValueRange(RPrevAP);

            CPTPrevCHDPrior = Variable.Array(Variable.Array<Dirichlet>(RAnyCHD), RTimeCHD).Named("CPTPrevCHDPrior");
            CPTPrevCHD = Variable.Array(Variable.Array<Vector>(RAnyCHD), RTimeCHD).Named("CPTPrevCHD");
            CPTPrevCHD[RTimeCHD][RAnyCHD] = Variable<Vector>.Random(CPTPrevCHDPrior[RTimeCHD][RAnyCHD]);
            CPTPrevCHD.SetValueRange(RPrevCHD);

            CPTPrevMIPrior = Variable.Array(Variable.Array<Dirichlet>(RHospMI), RTimeMI).Named("CPTPrevMIPrior");
            CPTPrevMI = Variable.Array(Variable.Array<Vector>(RHospMI), RTimeMI).Named("CPTPrevMI");
            CPTPrevMI[RTimeMI][RHospMI] = Variable<Vector>.Random(CPTPrevMIPrior[RTimeMI][RHospMI]);
            CPTPrevMI.SetValueRange(RPrevMI);

            CPTPrevStrkPrior = Variable.Array(Variable.Array<Dirichlet>(RStroke), RTimeStrk).Named("CPTPrevStrkPrior");
            CPTPrevStrk = Variable.Array(Variable.Array<Vector>(RStroke), RTimeStrk).Named("CPTPrevStrk");
            CPTPrevStrk[RTimeStrk][RStroke] = Variable<Vector>.Random(CPTPrevStrkPrior[RTimeStrk][RStroke]);
            CPTPrevStrk.SetValueRange(RPrevStrk);

            CPTPrevHypPrior = Variable.Array(Variable.Array<Dirichlet>(RHyperten), RTimeHyp).Named("CPTPrevHypPrior");
            CPTPrevHyp = Variable.Array(Variable.Array<Vector>(RHyperten), RTimeHyp).Named("CPTPrevHyp");
            CPTPrevHyp[RTimeHyp][RHyperten] = Variable<Vector>.Random(CPTPrevHypPrior[RTimeHyp][RHyperten]);
            CPTPrevHyp.SetValueRange(RPrevHyp);

            CPTDeathPrior = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet>(RAge), RAnyCHD), RHospMI), RMI_FCHD), RStroke).Named("CPTDeathPrior");
            CPTDeath = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RAge), RAnyCHD), RHospMI), RMI_FCHD), RStroke).Named("CPTDeath");
            CPTDeath[RStroke][RMI_FCHD][RHospMI][RAnyCHD][RAge] = Variable<Vector>.Random(CPTDeathPrior[RStroke][RMI_FCHD][RHospMI][RAnyCHD][RAge]);
            CPTDeath.SetValueRange(RDeath);

            CPTTimeDthPrior =Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Dirichlet> (RDeath), RTimeMI), RTimeMIFC), RTimeCHD), RTimeStrk).Named("CPTTimeDthPrior");
            CPTTimeDth = Variable.Array(Variable.Array(Variable.Array(Variable.Array(Variable.Array<Vector>(RDeath), RTimeMI), RTimeMIFC), RTimeCHD), RTimeStrk).Named("CPTTimeDth");
            CPTTimeDth[RTimeStrk][RTimeCHD][RTimeMIFC][RTimeMI][RDeath] = Variable<Vector>.Random(CPTTimeDthPrior[RTimeStrk][RTimeCHD][RTimeMIFC][RTimeMI][RDeath]);
            CPTTimeDth.SetValueRange(RTimeDth);

            #endregion

            #region DefinePrimaryVars
            // Define the primary variables
          /*  RandID = Variable.Array<int>(N).Named("RandID");
            RandID[N] = Variable.Discrete(ProbRandID).ForEach(N);*/
            Sex = Variable.Array<int>(N).Named("Sex");
            Sex[N] = Variable.Discrete(ProbSex).ForEach(N);
            Periodd = Variable.Array<int>(N).Named("Period");
            Periodd[N] = Variable.Discrete(ProbPeriodd).ForEach(N);
            BPMeds = Variable.Array<int>(N).Named("BPMeds");
            BPMeds[N] = Variable.Discrete(ProbBPMeds).ForEach(N);

            Age = AddChildFromTwoParents(Periodd, Sex, CPTAge).Named("Age");
            Time = AddChildFromOneParent(Periodd, CPTTime).Named("Time");
            BMI = AddChildFromTwoParents(Age,Sex, CPTBMI).Named("BMI");
            CurSmoke = AddChildFromTwoParents(Age, Sex, CPTCurSmoke).Named("CurSmoke");
            HDLC = AddChildFromOneParent(BMI, CPTHDLC).Named("HDLC");
            Diabetes = AddChildFromOneParent(BMI, CPTDiabetes).Named("Diabetes");
            Glucose = AddChildFromOneParent(Diabetes, CPTGlucose).Named("Glucose");
            CVD = AddChildFromOneParent(CurSmoke, CPTCVD).Named("CVD");
            SysBP = AddChildFromFourParents(BPMeds, CurSmoke, BMI, Age, CPTSysBP).Named("SysBP");
            DiaBP = AddChildFromThreeParents(CVD, Age,SysBP, CPTDiaBP).Named("DiaBP");
            Hyperten = AddChildFromTwoParents(DiaBP, Age, CPTHyperten).Named("Hyperten");
            LDLC = AddChildFromFourParents(Hyperten, CurSmoke, BMI, Diabetes, CPTLDLC).Named("LDLC");
            TotChol = AddChildFromFourParents(LDLC, HDLC, Sex, Age, CPTTotChol).Named("TotChol");
            Angina = AddChildFromFiveParents(Sex, CurSmoke, DiaBP, Diabetes, Age, CPTAngina).Named("Angina");
            MI_FCHD = AddChildFromSixParents(Age, Sex, CVD, TotChol,  DiaBP, Diabetes, CPTMI_FCHD).Named("MI_FCHD");
            HospMI = AddChildFromEightParents(Age, Sex, MI_FCHD, TotChol, CVD, DiaBP, Diabetes, Angina, CPTHospMI).Named("HospMI");
            AnyCHD = AddChildFromFiveParents(CVD, Angina, HospMI, DiaBP, Diabetes, CPTAnyCHD).Named("AnyCHD");
            Stroke = AddChildFromFourParents(Diabetes, DiaBP, CurSmoke, AnyCHD, CPTStroke).Named("Stroke");
            TimeAP = AddChildFromTwoParents(Angina, Time, CPTTimeAP).Named("TimeAP");
            TimeCVD = AddChildFromTwoParents(CVD, Time, CPTTimeCVD).Named("TimeCVD");
            TimeHyp = AddChildFromTwoParents(Hyperten, Time, CPTTimeHyp).Named("TimeHyp");
            TimeMIFC = AddChildFromThreeParents(TimeCVD,MI_FCHD, Time, CPTTimeMIFC).Named("TimeMIFC");
            TimeMI = AddChildFromFourParents(TimeMIFC,TimeAP,HospMI, Time, CPTTimeMI).Named("TimeMI");
            TimeCHD = AddChildFromFiveParents(TimeCVD,TimeAP,TimeMI,AnyCHD, Time, CPTTimeCHD).Named("TimeCHD");
            TimeStrk = AddChildFromThreeParents(TimeCHD,Stroke, Time, CPTTimeSTRK).Named("TimeSTRK");
            PrevAP = AddChildFromTwoParents(TimeAP,Angina,CPTPrevAP).Named("PrevAP");
            PrevCHD = AddChildFromTwoParents(TimeCHD, AnyCHD, CPTPrevCHD).Named("PrevCHD");
            PrevMI = AddChildFromTwoParents(TimeMI, HospMI, CPTPrevMI).Named("PrevMI");
            PrevStrk = AddChildFromTwoParents(TimeStrk, Stroke, CPTPrevStrk).Named("PrevStrk");
            PrevHyp = AddChildFromTwoParents(TimeHyp, Hyperten, CPTPrevHyp).Named("PrevHyp");
            Death = AddChildFromFiveParents(Stroke, MI_FCHD, HospMI, AnyCHD, Age, CPTDeath).Named("Death");
            TimeDth = AddChildFromFiveParents(TimeStrk,TimeCHD,TimeMIFC,TimeMI,Death, CPTTimeDth).Named("TimeDth");

            #endregion
        }

        //Learning
        public void LearnParameters(int count,
            int[] sex, int[] age, int[] sysbp, int[] diabp, int[] bpmeds, int[] cursmoke, int[]totchol, int[] hdlc, int[] ldlc,
            int[] bmi, int[] glucose, int[] diabetes, int[]prevap, int[] prevchd, int[] prevmi, int[] prevstrk, int[] prevhyp,
            int[] angina, int[] hospmi, int[] mi_fchd, int[] anychd, int[] stroke, int[] cvd, int[] hyperten, int[] death,
            int[] randid, int[] period, int[] time, int[] timeap, int[] timemi, int[] timemifc, int[] timechd,
            int[] timestrk, int[] timecvd, int[] timehyp, int[] timedth,  
            Dirichlet probBPMedsPrior, Dirichlet[][] cptPrevAPPrior, Dirichlet[][] cptPrevCHDPrior, Dirichlet[][] cptPrevMIPrior,
            Dirichlet[][] cptPrevStrkPrior, Dirichlet[][] cptPrevHypPrior, Dirichlet probRandIDPrior, Dirichlet probPeriodPrior,
            Dirichlet probSexPrior, Dirichlet[] cptHDLCPrior, 
            Dirichlet[] cptGlucosePrior, Dirichlet[] cptDiabetesPrior, Dirichlet[] cptTimePrior,
            Dirichlet[][] cptBMIPrior, Dirichlet[][] cptCurSmokePrior, 
            Dirichlet[][] cptTimeAPPrior, Dirichlet[][][][] cptTimeMIPrior, Dirichlet[][][] cptTimeMIFCPrior, Dirichlet[][][][][] cptTimeCHDPrior,
            Dirichlet[][] cptTimeCVDPrior, Dirichlet[][][] cptTimeStrkPrior, Dirichlet[][] cptTimeHypPrior,
            Dirichlet[][][] cptDiaBPPrior, Dirichlet[] cptCVDPrior, Dirichlet[][] cptHypertenPrior, Dirichlet[][] cptAgePrior, 
            Dirichlet[][][][] cptSysBPPrior, Dirichlet[][][][] cptTotCholPrior, Dirichlet[][][][] cptLDLCPrior,
            Dirichlet[][][][] cptStrokePrior, Dirichlet[][][][][] cptDeathPrior,
            Dirichlet[][][][][] cptAnginaPrior, Dirichlet[][][][][] cptAnyCHDPrior,
            Dirichlet[][][][][][] cptMI_FCHDPrior, Dirichlet[][][][][][][][] cptHospMIPrior, Dirichlet[][][][][] cptTimeDthPrior
            )
        {
            NumberOfExamples.ObservedValue = count;
           
            #region PrimaryVarObservedValue
           // RandID.ObservedValue = randid;
            Periodd.ObservedValue = period;
            Sex.ObservedValue = sex;
            Time.ObservedValue = time;
            BPMeds.ObservedValue = bpmeds;
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
            TimeAP.ObservedValue = timeap;
            TimeMI.ObservedValue = timemi;
            TimeMIFC.ObservedValue = timemifc;
            TimeCHD.ObservedValue = timechd;
            TimeStrk.ObservedValue = timestrk;
            TimeCVD.ObservedValue = timecvd;
            TimeHyp.ObservedValue = timehyp;
            PrevAP.ObservedValue = prevap;
            PrevCHD.ObservedValue = prevchd;
            PrevMI.ObservedValue = prevmi;
            PrevStrk.ObservedValue = prevstrk;
            PrevHyp.ObservedValue = prevhyp;
            Death.ObservedValue = death;
            TimeDth.ObservedValue = timedth;
            
            #endregion

            #region PriorProb&CPTObservedValue
          //  ProbRandIDPrior.ObservedValue = probRandIDPrior;
            ProbPerioddPrior.ObservedValue = probPeriodPrior;
            ProbBPMedsPrior.ObservedValue = probBPMedsPrior;
            ProbSexPrior.ObservedValue = probSexPrior;
            CPTAgePrior.ObservedValue = cptAgePrior;
            CPTTimePrior.ObservedValue = cptTimePrior;
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
            CPTTimeAPPrior.ObservedValue = cptTimeAPPrior;
            CPTTimeMIPrior.ObservedValue = cptTimeMIPrior;
            CPTTimeMIFCPrior.ObservedValue = cptTimeMIFCPrior;
            CPTTimeCHDPrior.ObservedValue = cptTimeCHDPrior;
            CPTTimeSTRKPrior.ObservedValue = cptTimeStrkPrior;
            CPTTimeCVDPrior.ObservedValue = cptTimeCVDPrior;
            CPTTimeHypPrior.ObservedValue = cptTimeHypPrior;
            CPTPrevAPPrior.ObservedValue = cptPrevAPPrior;
            CPTPrevCHDPrior.ObservedValue = cptPrevCHDPrior;
            CPTPrevMIPrior.ObservedValue = cptPrevMIPrior;
            CPTPrevStrkPrior.ObservedValue = cptPrevStrkPrior;
            CPTPrevHypPrior.ObservedValue = cptPrevHypPrior;
            CPTDeathPrior.ObservedValue = cptDeathPrior;
            CPTTimeDthPrior.ObservedValue = cptTimeDthPrior;
            #endregion

            // Inference

           // ProbRandIDPosterior = Engine.Infer<Dirichlet>(ProbRandID);
            ProbSexPosterior = Engine.Infer<Dirichlet>(ProbSex);
            ProbPerioddPosterior = Engine.Infer<Dirichlet>(ProbPeriodd);
            ProbBPMedsPosterior = Engine.Infer<Dirichlet>(ProbBPMeds);
           
            CPTTimePosterior = Engine.Infer<Dirichlet[]>(CPTTime);
            CPTAgePosterior = Engine.Infer<Dirichlet[][]>(CPTAge);
            CPTBMIPosterior = Engine.Infer<Dirichlet[][]>(CPTBMI);
            CPTCurSmokePosterior = Engine.Infer<Dirichlet[][]>(CPTCurSmoke);
            CPTHDLCPosterior = Engine.Infer<Dirichlet[]>(CPTHDLC);
            CPTDiabetesPosterior = Engine.Infer<Dirichlet[]>(CPTDiabetes);
            CPTGlucosePosterior = Engine.Infer<Dirichlet[]>(CPTGlucose);
            CPTCVDPosterior = Engine.Infer<Dirichlet[]>(CPTCVD);
            CPTSysBPPosterior = Engine.Infer<Dirichlet[][][][]>(CPTSysBP);
            CPTDiaBPPosterior = Engine.Infer<Dirichlet[][][]>(CPTDiaBP);
            CPTHypertenPosterior = Engine.Infer<Dirichlet[][]>(CPTHyperten);
            CPTLDLCPosterior = Engine.Infer<Dirichlet[][][][]>(CPTLDLC);
            CPTTotCholPosterior = Engine.Infer<Dirichlet[][][][]>(CPTTotChol);
            CPTAnginaPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTAngina);
            CPTMI_FCHDPosterior = Engine.Infer<Dirichlet[][][][][][]>(CPTMI_FCHD);
            CPTHospMIPosterior = Engine.Infer<Dirichlet[][][][][][][][]>(CPTHospMI);
            CPTAnyCHDPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTAnyCHD);
            CPTStrokePosterior = Engine.Infer<Dirichlet[][][][]>(CPTStroke);
            CPTTimeAPPosterior = Engine.Infer<Dirichlet[][]>(CPTTimeAP);
            CPTTimeCVDPosterior = Engine.Infer<Dirichlet[][]>(CPTTimeCVD);
            CPTTimeHypPosterior = Engine.Infer<Dirichlet[][]>(CPTTimeHyp);
            CPTTimeMIFCPosterior = Engine.Infer<Dirichlet[][][]>(CPTTimeMIFC);
            CPTTimeMIPosterior = Engine.Infer<Dirichlet[][][][]>(CPTTimeMI);
            CPTTimeCHDPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTTimeCHD);
            CPTTimeSTRKPosterior = Engine.Infer<Dirichlet[][][]>(CPTTimeSTRK);
            CPTPrevAPPosterior = Engine.Infer<Dirichlet[][]>(CPTPrevAP);
            CPTPrevCHDPosterior = Engine.Infer<Dirichlet[][]>(CPTPrevCHD);
            CPTPrevMIPosterior = Engine.Infer<Dirichlet[][]>(CPTPrevMI);
            CPTPrevStrkPosterior = Engine.Infer<Dirichlet[][]>(CPTPrevStrk);
            CPTPrevHypPosterior = Engine.Infer<Dirichlet[][]>(CPTPrevHyp);

            CPTDeathPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTDeath);
            CPTTimeDthPosterior = Engine.Infer<Dirichlet[][][][][]>(CPTTimeDth);
        }

        public void LearnParameters(int count,
            int[] sex, int[] age, int[] sysbp, int[] diabp, int[] bpmeds, int[] cursmoke, int[]totchol, int[] hdlc, int[] ldlc,
            int[] bmi, int[] glucose, int[] diabetes, int[]prevap, int[] prevchd, int[] prevmi, int[] prevstrk, int[] prevhyp,
            int[] angina, int[] hospmi, int[] mi_fchd, int[] anychd, int[] stroke, int[] cvd, int[] hyperten, int[] death,
            int[] randid, int[] period, int[] time, int[] timeap, int[] timemi, int[] timemifc, int[] timechd,
            int[] timestrk, int[] timecvd, int[] timehyp, int[] timedth
            )
        {
            // Set all priors to uniform
           // Dirichlet probRandIDPrior = Dirichlet.Uniform(1);
            Dirichlet probSexPrior = Dirichlet.Uniform(2);
            Dirichlet probPeriodPrior = Dirichlet.Uniform(3);
            Dirichlet probBPMedsPrior = Dirichlet.Uniform(2);

            Dirichlet[][] cptAgePrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(5), 2).ToArray(), 3).ToArray();         //5 2 9999312 3
            Dirichlet[] cptTimePrior = Enumerable.Repeat(Dirichlet.Uniform(3), 3).ToArray();         //3 4854
            Dirichlet[][] cptBMIPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(7), 2).ToArray(), 5).ToArray();     //7 5 2                
            Dirichlet[][] cptCurSmokePrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 5).ToArray(); ; //2 2 5
            Dirichlet[] cptHDLCPrior = Enumerable.Repeat(Dirichlet.Uniform(3), 7).ToArray();        //3 7                     
            Dirichlet[] cptDiabetesPrior = Enumerable.Repeat(Dirichlet.Uniform(2), 7).ToArray();    //2 7  
            Dirichlet[] cptGlucosePrior = Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray();     //2 2                  
            Dirichlet[] cptCVDPrior = Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray();     //2 2        
            Dirichlet[][][][] cptSysBPPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(5), 5).ToArray(), 7).ToArray(), 2).ToArray(), 2).ToArray();  //5 2 7 5 2                   
            Dirichlet[][][] cptDiaBPPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 5).ToArray(), 5).ToArray(), 2).ToArray();   //6 5 5 2        
            Dirichlet[][] cptHypertenPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 6).ToArray(); //2 2 5 6
            Dirichlet[][][][] cptLDLCPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(3), 2).ToArray(), 7).ToArray(), 2).ToArray(), 2).ToArray();     //3 7 2 2 2                 
            Dirichlet[][][][] cptTotCholPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(3), 5).ToArray(), 2).ToArray(), 3).ToArray(), 3).ToArray(); //3 2 5 3 3                  
            Dirichlet[][][][][] cptAnginaPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 2).ToArray();   //2 2 2 2 2 5 6
            Dirichlet[][][][][][]cptMI_FCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray(), 3).ToArray(), 2).ToArray(), 2).ToArray(), 5).ToArray();//2 2 2 2 2 5 5 3 2
            Dirichlet[][][][][][][][] cptHospMIPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(), 3).ToArray(), 2).ToArray(), 2).ToArray(), 5).ToArray();    //2 2 2 2 2 5 6 3 2
            Dirichlet[][][][][] cptAnyCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray();  //2 2 2 2 2 6 2          
            Dirichlet[][][][] cptStrokePrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 2).ToArray(), 6).ToArray(), 2).ToArray(); //2 2 2 2 2 6
            Dirichlet[][] cptTimeAPPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6),3).ToArray(), 2).ToArray();     //8766 2 4854               
            Dirichlet[][] cptTimeCVDPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(), 2).ToArray();     //8766 2 4854
            Dirichlet[][] cptTimeHypPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(), 2).ToArray();     //8766 2 4854
            Dirichlet[][][] cptTimeMIFCPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(),2).ToArray(), 6).ToArray();     //8766 2 4854
            Dirichlet[][][][] cptTimeMIPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(), 2).ToArray(), 6).ToArray(), 6).ToArray();     //8766 2 4854
            Dirichlet[][][][][] cptTimeCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(), 2).ToArray(), 6).ToArray(), 6).ToArray(), 6).ToArray();     //8766 2 4854
            Dirichlet[][][] cptTimeStrkPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 3).ToArray(), 2).ToArray(), 6).ToArray();     //8766 2 4854
            Dirichlet[][] cptPrevAPPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray();
            Dirichlet[][] cptPrevCHDPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray();
            Dirichlet[][] cptPrevMIPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray();
            Dirichlet[][] cptPrevStrkPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray();
            Dirichlet[][] cptPrevHypPrior = Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 2).ToArray(), 6).ToArray(); 
            Dirichlet[][][][][] cptDeathPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(2), 5).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray(), 2).ToArray();   // 2 2 2 2 2 5             
            Dirichlet[][][][][] cptTimeDthPrior = Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat(Dirichlet.Uniform(6), 2).ToArray(), 6).ToArray(), 6).ToArray(), 6).ToArray(), 6).ToArray();    
           
            LearnParameters(count,sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, glucose, diabetes,
                prevap, prevchd, prevmi, prevstrk, prevhyp, angina, hospmi, mi_fchd, anychd, stroke, cvd, hyperten, death,
                randid, period, time, timeap, timemi, timemifc, timechd, timestrk,  timecvd, timehyp, timedth,
                probBPMedsPrior, cptPrevAPPrior, cptPrevCHDPrior, cptPrevMIPrior, cptPrevStrkPrior, cptPrevHypPrior,
                null, probPeriodPrior,
                probSexPrior, cptHDLCPrior, cptGlucosePrior, cptDiabetesPrior, cptTimePrior, cptBMIPrior, cptCurSmokePrior, 
                cptTimeAPPrior, cptTimeMIPrior, cptTimeMIFCPrior, cptTimeCHDPrior, cptTimeCVDPrior, cptTimeStrkPrior, 
                cptTimeHypPrior, cptDiaBPPrior, cptCVDPrior, cptHypertenPrior, cptAgePrior, cptSysBPPrior, 
                cptTotCholPrior, cptLDLCPrior, cptStrokePrior, cptDeathPrior, cptAnginaPrior, cptAnyCHDPrior,
                cptMI_FCHDPrior, cptHospMIPrior, cptTimeDthPrior
                );     
        }
                                                                    
        //Probility 
        //mode - which probability is need   
        //probmode: true-diagnostic, false-prevention                    
        public double ProbDisease(bool probmode, int mode,
            int? sex, int? age, int? sysbp, int? diabp, int? bpmeds, int? cursmoke, int? totchol, int? hdlc, int? ldlc,
            int? bmi, int? glucose, int? diabetes, int? prevap, int? prevchd, int? prevmi, int? prevstrk, int? prevhyp,
            int? angina, int? hospmi, int? mi_fchd, int? anychd, int? stroke, int? cvd, int? hyperten, int? death,
            int? randid, int? period, int? time, int? timeap, int? timemi, int? timemifc, int? timechd,
            int? timestrk, int? timecvd, int? timehyp, int? timedth,
            Dirichlet probSexPrior, Dirichlet probBPMedsPrior, Dirichlet[][] cptPrevAPPrior, Dirichlet[][] cptPrevCHDPrior, Dirichlet[][] cptPrevMIPrior,
            Dirichlet[][] cptPrevStrkPrior, Dirichlet[][] cptPrevHypPrior,
            Dirichlet[][] cptAgePrior, Dirichlet[] cptHDLCPrior, Dirichlet[] cptGlucosePrior, Dirichlet[] cptDiabetesPrior,
            Dirichlet[][] cptBMIPrior, Dirichlet[][] cptCurSmokePrior,
            Dirichlet[][][] cptDiaBPPrior, Dirichlet[] cptCVDPrior, Dirichlet[][] cptHypertenPrior,
            Dirichlet[][][][] cptSysBPPrior, Dirichlet[][][][] cptTotCholPrior, Dirichlet[][][][] cptLDLCPrior,
            Dirichlet[][][][]cptStrokePrior, Dirichlet[][][][][] cptDeathPrior,
            Dirichlet[][][][][] cptAnginaPrior, Dirichlet[][][][][] cptAnyCHDPrior,
            Dirichlet[][][][][][]cptMI_FCHDPrior, Dirichlet[][][][][][][][] cptHospMIPrior,
            Dirichlet probRandIDPrior, Dirichlet probPeriodPrior, Dirichlet[] cptTimePrior,Dirichlet[][] cptTimeAPPrior,
            Dirichlet[][][][] cptTimeMIPrior,Dirichlet[][][] cptTimeMIFCPrior,Dirichlet[][][][][] cptTimeCHDPrior, Dirichlet[][] cptTimeCVDPrior,
            Dirichlet[][][] cptTimeStrkPrior, Dirichlet[][] cptTimeHypPrior, Dirichlet[][][][][] cptTimeDthPrior
            )
        {
            NumberOfExamples.ObservedValue = 1;                                            
                                                                                           
            #region SetOrClearObservedValue                                               
            
           // if (randid.HasValue) RandID.ObservedValue = new int[] { randid.Value }; else RandID.ClearObservedValue();
            if (period.HasValue) Periodd.ObservedValue = new int[] { period.Value }; else Periodd.ClearObservedValue();                                                     
            if (sex.HasValue) Sex.ObservedValue = new int[] { sex.Value };  else Sex.ClearObservedValue();
            if (time.HasValue) Time.ObservedValue = new int[] { time.Value }; else Time.ClearObservedValue();
            if (bpmeds.HasValue) BPMeds.ObservedValue = new int[] { bpmeds.Value }; else BPMeds.ClearObservedValue();
            if (age.HasValue) Age.ObservedValue = new int[] { age.Value }; else Age.ClearObservedValue();
            if (bmi.HasValue) BMI.ObservedValue = new int[] { bmi.Value }; else BMI.ClearObservedValue();
            if (cursmoke.HasValue) CurSmoke.ObservedValue = new int[] { cursmoke.Value }; else CurSmoke.ClearObservedValue();
            if (hdlc.HasValue) HDLC.ObservedValue = new int[] { hdlc.Value }; else HDLC.ClearObservedValue();
            if (diabetes.HasValue) Diabetes.ObservedValue = new int[] { diabetes.Value }; else Diabetes.ClearObservedValue();
            if (glucose.HasValue) Glucose.ObservedValue = new int[] { glucose.Value }; else Glucose.ClearObservedValue();
            if (cvd.HasValue && probmode && mode != (int)modes.cvd) CVD.ObservedValue = new int[] { cvd.Value };else CVD.ClearObservedValue();
            if (timecvd.HasValue) TimeCVD.ObservedValue = new int[] { timecvd.Value }; else TimeCVD.ClearObservedValue();
            if (sysbp.HasValue) SysBP.ObservedValue = new int[] { sysbp.Value }; else SysBP.ClearObservedValue();
            if (diabp.HasValue) DiaBP.ObservedValue = new int[] { diabp.Value }; else DiaBP.ClearObservedValue();
            if (hyperten.HasValue && probmode && mode != (int)modes.hyperten) Hyperten.ObservedValue = new int[] { hyperten.Value }; else Hyperten.ClearObservedValue();
            if (timehyp.HasValue) TimeHyp.ObservedValue = new int[] { timehyp.Value }; else TimeHyp.ClearObservedValue();
            if (ldlc.HasValue) LDLC.ObservedValue = new int[] { ldlc.Value }; else LDLC.ClearObservedValue();
            if (totchol.HasValue) TotChol.ObservedValue = new int[] { totchol.Value }; else TotChol.ClearObservedValue();
            if (angina.HasValue && probmode && mode != (int)modes.angina) Angina.ObservedValue = new int[] { angina.Value }; else Angina.ClearObservedValue();
            if (timeap.HasValue) TimeAP.ObservedValue = new int[] { timeap.Value }; else TimeAP.ClearObservedValue();
            if (mi_fchd.HasValue && probmode && mode != (int)modes.mi_fdch) MI_FCHD.ObservedValue = new int[] { mi_fchd.Value }; else MI_FCHD.ClearObservedValue();
            if (timemifc.HasValue) TimeMIFC.ObservedValue = new int[] { timemifc.Value }; else TimeMIFC.ClearObservedValue();
            if (hospmi.HasValue && probmode && mode != (int)modes.hospmi) HospMI.ObservedValue = new int[] { hospmi.Value }; else HospMI.ClearObservedValue();
            if (timemi.HasValue) TimeMI.ObservedValue = new int[] { timemi.Value }; else TimeMI.ClearObservedValue();
            if (anychd.HasValue && probmode && mode != (int)modes.anychd) AnyCHD.ObservedValue = new int[] { anychd.Value }; else AnyCHD.ClearObservedValue();
            if (timechd.HasValue) TimeCHD.ObservedValue = new int[] { timechd.Value }; else TimeCHD.ClearObservedValue();
            if (stroke.HasValue && probmode && mode != (int)modes.stroke) Stroke.ObservedValue = new int[] { stroke.Value }; else Stroke.ClearObservedValue();
            if (timestrk.HasValue) TimeStrk.ObservedValue = new int[] { timestrk.Value }; else TimeStrk.ClearObservedValue();
            if (prevap.HasValue && probmode) PrevAP.ObservedValue = new int[] { prevap.Value }; else PrevAP.ClearObservedValue();
            if (prevchd.HasValue && probmode) PrevCHD.ObservedValue = new int[] { prevchd.Value }; else PrevCHD.ClearObservedValue();
            if (prevmi.HasValue && probmode) PrevMI.ObservedValue = new int[] { prevmi.Value }; else PrevMI.ClearObservedValue();
            if (prevstrk.HasValue && probmode) PrevStrk.ObservedValue = new int[] { prevstrk.Value }; else PrevStrk.ClearObservedValue();
            if (prevhyp.HasValue && probmode) PrevHyp.ObservedValue = new int[] { prevhyp.Value }; else PrevHyp.ClearObservedValue();
            if (death.HasValue && probmode && mode != (int)modes.death) Death.ObservedValue = new int[] { death.Value }; else Death.ClearObservedValue();
            if (timedth.HasValue) TimeDth.ObservedValue = new int[] { timedth.Value }; else TimeDth.ClearObservedValue();
            
            #endregion

            #region SetProb&CPTPriorObservedValue

            //ProbRandIDPrior.ObservedValue = probRandIDPrior;
            ProbPerioddPrior.ObservedValue = probPeriodPrior;
            ProbBPMedsPrior.ObservedValue = probBPMedsPrior;
            ProbSexPrior.ObservedValue = probSexPrior;
            CPTAgePrior.ObservedValue = cptAgePrior;
            CPTTimePrior.ObservedValue = cptTimePrior;
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
            CPTTimeAPPrior.ObservedValue = cptTimeAPPrior;
            CPTTimeMIPrior.ObservedValue = cptTimeMIPrior;
            CPTTimeMIFCPrior.ObservedValue=cptTimeMIFCPrior;
            CPTTimeCHDPrior.ObservedValue = cptTimeCHDPrior;
            CPTTimeSTRKPrior.ObservedValue = cptTimeStrkPrior;
            CPTTimeCVDPrior.ObservedValue = cptTimeCVDPrior;
            CPTTimeHypPrior.ObservedValue = cptTimeHypPrior;
            CPTPrevAPPrior.ObservedValue = cptPrevAPPrior;
            CPTPrevCHDPrior.ObservedValue = cptPrevCHDPrior;
            CPTPrevMIPrior.ObservedValue = cptPrevMIPrior;
            CPTPrevStrkPrior.ObservedValue = cptPrevStrkPrior;
            CPTPrevHypPrior.ObservedValue = cptPrevHypPrior;
            CPTDeathPrior.ObservedValue = cptDeathPrior;
            CPTTimeDthPrior.ObservedValue = cptTimeDthPrior;
            
            #endregion

            // Inference
            switch(mode)
            {
                case 1:
                    if (probmode)
                    {
                        var anginaPosterior = Engine.Infer<Discrete[]>(Angina);
                        return anginaPosterior[0].GetProbs()[1];
                    }
                    else
                    {
                        var prevAPPosterior = Engine.Infer<Discrete[]>(PrevAP);
                        return prevAPPosterior[0].GetProbs()[1];
                    }
                    
                case 2:
                    var mi_fchdPosterior = Engine.Infer<Discrete[]>(MI_FCHD);
                    return mi_fchdPosterior[0].GetProbs()[1];
                case 3:
                    if (probmode)
                    {
                        var hospmiPosterior = Engine.Infer<Discrete[]>(HospMI);
                        return hospmiPosterior[0].GetProbs()[1];
                    }
                    else
                    {
                        var prevMIPosterior = Engine.Infer<Discrete[]>(PrevMI);
                        return prevMIPosterior[0].GetProbs()[1];
                    }
                case 4:
                    if (probmode)
                    {
                        var anychdPosterior = Engine.Infer<Discrete[]>(AnyCHD);
                        return anychdPosterior[0].GetProbs()[1];
                    }
                    else
                    {
                        var prevCHDPosterior = Engine.Infer<Discrete[]>(PrevCHD);
                        return prevCHDPosterior[0].GetProbs()[1];
                    }
                    
                case 5:
                    if (probmode)
                    {
                        var strokePosterior = Engine.Infer<Discrete[]>(Stroke);
                        return strokePosterior[0].GetProbs()[1];
                    }
                    else
                    {
                        var prevstrkPosterior = Engine.Infer<Discrete[]>(PrevStrk);
                        return prevstrkPosterior[0].GetProbs()[1];
                    }
                  
                case 6:
                    var cvdPosterior = Engine.Infer<Discrete[]>(CVD);
                    return cvdPosterior[0].GetProbs()[1];
                case 7:
                    if (probmode)
                    {
                        var hypertenPosterior = Engine.Infer<Discrete[]>(Hyperten);
                        return hypertenPosterior[0].GetProbs()[1];
                    }
                    else
                    {
                        var prevhypPosterior = Engine.Infer<Discrete[]>(PrevHyp);
                        return prevhypPosterior[0].GetProbs()[1];
                    }
                case 8:
                    var deathPosterior = Engine.Infer<Discrete[]>(Death);
                    return deathPosterior[0].GetProbs()[1];
                default:
                    throw new Exception();
            }                  
        }

        public double ProbDisease(bool probmode,int mode,
            int? sex, int? age, int? sysbp, int? diabp, int? bpmeds, int? cursmoke, int? totchol, int? hdlc, int? ldlc,
            int? bmi, int? glucose, int? diabetes, int? prevap, int? prevchd, int? prevmi, int? prevstrk, int? prevhyp,
            int? angina, int? hospmi, int? mi_fchd, int? anychd, int? stroke, int? cvd, int? hyperten, int? death,
            int? randid, int? period, int? time, int? timeap, int? timemi, int? timemifc, int? timechd,
            int? timestrk, int? timecvd, int? timehyp, int? timedth,
            Vector probSex, Vector probBPMeds, Vector[][] cptPrevAP, Vector[][] cptPrevCHD, Vector[][] cptPrevMI,Vector[][] cptPrevStrk, Vector[][] cptPrevHyp,
            Vector[][] cptAge, Vector[] cptHDLC, Vector[] cptGlucose, Vector[] cptDiabetes, Vector[][] cptBMI, Vector[][] cptCurSmoke,
            Vector[][][] cptDiaBP, Vector[] cptCVD, Vector[][] cptHyperten, Vector[][][][] cptSysBP, Vector[][][][] cptTotChol, Vector[][][][] cptLDLC,
            Vector[][][][]cptStroke, Vector[][][][][] cptDeath, Vector[][][][][] cptAngina, Vector[][][][][] cptAnyCHD,
            Vector[][][][][][] cptMI_FCHD, Vector[][][][][][][][] cptHospMI,
            Vector probRandID, Vector probPeriod, Vector[] cptTime, Vector[][] cptTimeAP,
            Vector[][][][] cptTimeMI, Vector[][][] cptTimeMIFC, Vector[][][][][] cptTimeCHD, Vector[][] cptTimeCVD,
            Vector[][][] cptTimeStrk, Vector[][] cptTimeHyp, Vector[][][][][]cptTimeDth
            )
        {
            //var probRandIDPrior = Dirichlet.PointMass(probRandID);
            var probPeriodPrior = Dirichlet.PointMass(probPeriod);
            var probSexPrior = Dirichlet.PointMass(probSex);
            var probBPMedsPrior = Dirichlet.PointMass(probBPMeds);
            
            var cptAgePrior = cptAge.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptTimePrior = cptTime.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptHDLCPrior = cptHDLC.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptGlucosePrior = cptGlucose.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptDiabetesPrior = cptDiabetes.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptBMIPrior = cptBMI.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptCurSmokePrior = cptCurSmoke.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptDiaBPPrior = cptDiaBP.Select(v3=>v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptCVDPrior = cptCVD.Select(v1 => Dirichlet.PointMass(v1)).ToArray();
            var cptHypertenPrior = cptHyperten.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptSysBPPrior = cptSysBP.Select(v4=>v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptTotCholPrior = cptTotChol.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptLDLCPrior = cptLDLC.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptStrokePrior = cptStroke.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptDeathPrior = cptDeath.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptAnginaPrior = cptAngina.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptAnyCHDPrior = cptAnyCHD.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptMI_FCHDPrior = cptMI_FCHD.Select(v6 => v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptTimeAPPrior = cptTimeAP.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptTimeCVDPrior = cptTimeCVD.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptTimeHypPrior = cptTimeHyp.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptTimeMIFCPrior = cptTimeMIFC.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptTimeMIPrior = cptTimeMI.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray();
            var cptTimeCHDPrior = cptTimeCHD.Select(v5=>v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var cptTimeStrkPrior = cptTimeStrk.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray();
            var cptHospMIPrior=cptHospMI.Select(v8=>v8.Select(v7=>v7.Select(v6 => v6.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            var probPrevAPPrior = cptPrevAP.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var probPrevCHDPrior = cptPrevCHD.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var probPrevMIPrior = cptPrevMI.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var probPrevStrkPrior = cptPrevStrk.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var probPrevHypPrior = cptPrevHyp.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray();
            var cptTimeDthPrior = cptTimeDth.Select(v5 => v5.Select(v4 => v4.Select(v3 => v3.Select(v2 => v2.Select(v1 => Dirichlet.PointMass(v1)).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();
            return ProbDisease(probmode,mode,
                sex, age, sysbp, diabp, bpmeds, cursmoke, totchol, hdlc, ldlc, bmi, glucose, diabetes, prevap, prevchd,
                prevmi, prevstrk, prevhyp, angina, hospmi, mi_fchd, anychd, stroke, cvd, hyperten, death,
                randid, period, time, timeap, timemi, timemifc, timechd, timestrk, timecvd, timehyp, timedth,
                probSexPrior, probBPMedsPrior, probPrevAPPrior, probPrevCHDPrior, probPrevMIPrior, probPrevStrkPrior, probPrevHypPrior,
                cptAgePrior, cptHDLCPrior, cptGlucosePrior, cptDiabetesPrior, cptBMIPrior, cptCurSmokePrior, cptDiaBPPrior,
                cptCVDPrior, cptHypertenPrior, cptSysBPPrior, cptTotCholPrior, cptLDLCPrior, cptStrokePrior, cptDeathPrior,
                cptAnginaPrior, cptAnyCHDPrior, cptMI_FCHDPrior, cptHospMIPrior,
                null, probPeriodPrior, cptTimePrior, cptTimeAPPrior, cptTimeMIPrior, cptTimeMIFCPrior,
                cptTimeCHDPrior, cptTimeCVDPrior, cptTimeStrkPrior, cptTimeHypPrior, cptTimeDthPrior
                );
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

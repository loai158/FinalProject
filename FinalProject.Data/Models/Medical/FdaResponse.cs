namespace FinalProject.Data.Models.Medical
{
    public class AdverseEventResponse
    {
        public Meta meta { get; set; }
        public AdverseEventResult[] results { get; set; }
    }

    public class AdverseEventResult
    {
        // حقول مهمة ممكن تلاقيها
        public Patient patient { get; set; }
        public List<Reaction> reaction { get; set; } // الأعراض الجانبية المبلغ عنها في هذا الحدث

        // وهناك حقول أخرى كثيرة تتعلق ببيانات الإبلاغ
    }

    public class Patient
    {
        public string patientsex { get; set; }
        public int patientonsetage { get; set; }
        public string patientonsetageunit { get; set; }
        public List<Drug> drug { get; set; } // الأدوية التي أخذها المريض في هذا الحدث
        // أضف حقول أخرى مثل patientweight, patientrace, etc.
    }

    public class Drug
    {
        public string medicinalproduct { get; set; } // اسم الدواء المبلغ عنه
        public List<Openfda> openfda { get; set; } // يحتوي على brand_name, generic_name, rxcui
        // أضف حقول أخرى مثل drugdosage, drugstructurededosagenumb, drugadministrationroute
    }

    public class Openfda
    {
        public List<string> brand_name { get; set; }
        public List<string> generic_name { get; set; }
        public List<string> rxcui { get; set; } // مفيد جداً للمطابقة الدقيقة
        // أضف أي حقول openfda أخرى تهمك
    }

    public class Reaction
    {
        public string reactionmeddrapt { get; set; } // وصف الحدث السلبي (مصطلح طبي موحد)
        public string reactionmeddraversionpt { get; set; }
    }

    // نفس نماذج Meta و Results من قبل
    public class Meta
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public string last_updated { get; set; }
        public Results results { get; set; }
    }

    public class Results
    {
        public int skip { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
    }
    public class DrugLabelResponse
    {
        public MetaData meta { get; set; }
        public List<DrugLabelResult> results { get; set; }
    }

    public class MetaData
    {
        public string disclaimer { get; set; }
        public string terms { get; set; }
        public string license { get; set; }
        public string last_updated { get; set; }
        public ResultsInfo results { get; set; }
    }

    public class ResultsInfo
    {
        public int skip { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
    }

    public class DrugLabelResult
    {
        public List<string> spl_product_data_elements { get; set; }
        public List<string> active_ingredient { get; set; }
        public List<string> purpose { get; set; }
        public List<string> indications_and_usage { get; set; }
        public List<string> warnings { get; set; } // دعم حقل warnings
        public List<string> do_not_use { get; set; }
        public List<string> when_using { get; set; }
        public List<string> stop_use { get; set; }
        public List<string> keep_out_of_reach_of_children { get; set; }
        public List<string> dosage_and_administration { get; set; }
        public List<string> storage_and_handling { get; set; }
        public List<string> inactive_ingredient { get; set; }
        public List<string> questions { get; set; }
        public List<string> package_label_principal_display_panel { get; set; }
        public string set_id { get; set; }
        public string effective_time { get; set; }
        public string version { get; set; }
        public OpenFda openfda { get; set; }
    }

    public class OpenFda
    {
        public List<string> application_number { get; set; }
        public List<string> brand_name { get; set; }
        public List<string> generic_name { get; set; }
        public List<string> manufacturer_name { get; set; }
        public List<string> product_ndc { get; set; }
        public List<string> product_type { get; set; }
        public List<string> route { get; set; }
        public List<string> substance_name { get; set; }
        public List<string> rxcui { get; set; }
        public List<string> spl_id { get; set; }
        public List<string> spl_set_id { get; set; }
        public List<string> package_ndc { get; set; }
        public List<bool> is_original_packager { get; set; }
        public List<string> nui { get; set; }
        public List<string> pharm_class_epc { get; set; }
        public List<string> pharm_class_moa { get; set; }
        public List<string> unii { get; set; }
    }
}
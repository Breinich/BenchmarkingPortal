let Settings = {
    validClass: "is-valid",
    errorClass: "is-invalid"
};

$.validator.setDefaults(Settings);
$.validator.unobtrusive.options = Settings;
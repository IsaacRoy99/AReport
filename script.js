const reportForm = document.getElementById("reportForm");

reportForm.addEventListener("submit", function(e){

    e.preventDefault();

    // Inputs
    const title = document.querySelector('input[type="text"]').value.trim();
    const department = document.querySelector('select').value.trim();
    const summary = document.querySelectorAll('textarea')[0].value.trim();
    const challenges = document.querySelectorAll('textarea')[1].value.trim();
    const progress = document.querySelectorAll('textarea')[2].value.trim();

    // Validation
    if(title === "" || summary === "" || challenges === "" || progress === ""){

        Swal.fire({
            title: "Missing Information",
            text: "Please complete all required fields before submitting the report.",
            icon: "error",
            confirmButtonText: "Okay",
            confirmButtonColor: "#d33",
            background: "#ffffff"
        });

        return;
    }

    // Success Alert
    Swal.fire({
        title: "Report Submitted!",
        html: `
            <b>Your department report was submitted successfully.</b>
            <br><br>
            The report has been forwarded for managerial review.
        `,
        icon: "success",
        confirmButtonText: "Close",
        confirmButtonColor: "#fca311",
        background: "#ffffff",
        allowOutsideClick: false
    });

    // Reset Form
    reportForm.reset();

});
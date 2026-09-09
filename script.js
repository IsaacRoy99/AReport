// ================================
// REPORT SYSTEM
// ================================

const reportForm = document.getElementById("reportForm");

function getStatusBadge(status) {
    if (status === "Completed") return "success";
    if (status === "Blocked") return "danger";
    if (status === "On Hold") return "warning";

    return "primary";
}

// ================================
// DASHBOARD ACCESS CHECK
// ================================

if (
    window.location.pathname.endsWith("index.html") &&
    sessionStorage.getItem("loggedIn") !== "true"
) {
    window.location.href = "login.html";
}

if (
    (window.location.pathname.endsWith("/") ||
     window.location.pathname.endsWith("index.html")) &&
    sessionStorage.getItem("loggedIn") !== "true"
) {
    window.location.href = "login.html";
}


function loadReports() {

    const table = document.getElementById("reportTable");

    // Only run on dashboard
    if (!table) return;

    let reports = JSON.parse(localStorage.getItem("reports")) || [];

    table.innerHTML = "";

    reports.forEach(report => {

        table.innerHTML += `
            <tr>
                <td>${report.title}</td>

                <td>${report.team}</td>

                <td>
                    <span class="badge bg-${getStatusBadge(report.status)}">
                        ${report.status}
                    </span>
                </td>

                <td>${report.summary}</td>
            </tr>
        `;
    });
}


// ================================
// SUBMIT REPORT
// ================================

if (reportForm) {

    reportForm.addEventListener("submit", function(e) {

        e.preventDefault();

        const title = document.getElementById("reportTitle").value.trim();
        const team = document.getElementById("team").value.trim();
        const status = document.getElementById("status").value.trim();

        const textareas = document.querySelectorAll("#reportForm textarea");

        const summary = textareas[0].value.trim();
        const challenges = textareas[1].value.trim();
        const progress = textareas[2].value.trim();


        // Validation
        if (
            title === "" ||
            team === "" ||
            status === "" ||
            summary === "" ||
            challenges === "" ||
            progress === ""
        ) {

            Swal.fire({
                title: "Missing Information",
                text: "Please complete all required fields before submitting.",
                icon: "error",
                confirmButtonColor: "#d33"
            });

            return;
        }


        // Create report
        const report = {
            title: title,
            team: team,
            status: status,
            summary: summary
        };


        // Get existing reports
        let reports = JSON.parse(localStorage.getItem("reports")) || [];

        // Add new report
        reports.push(report);

        // Save reports
        localStorage.setItem("reports", JSON.stringify(reports));


        // Refresh table
        loadReports();


        // Success message
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


        // Clear form
        reportForm.reset();
    });
}


// Load reports when dashboard opens
loadReports();
updateStats();

function updateStats() {

    const reports = JSON.parse(localStorage.getItem("reports")) || [];

    const submitted = document.getElementById("reportsSubmitted");
    const pending = document.getElementById("pendingReviews");
    const approved = document.getElementById("approvedReports");

    if (!submitted || !pending || !approved) return;


    const submittedCount = reports.length;

    const pendingCount = reports.filter(report =>
        report.status !== "Completed"
    ).length;

    const approvedCount = reports.filter(report =>
        report.status === "Completed"
    ).length;


    submitted.textContent = submittedCount;
    pending.textContent = pendingCount;
    approved.textContent = approvedCount;
}


// ================================
// SEARCH REPORTS
// ================================

const searchInput = document.getElementById("searchInput");

if (searchInput) {

    searchInput.addEventListener("keyup", function() {

        const value = this.value.toLowerCase();

        const rows = document.querySelectorAll("#reportTable tr");

        rows.forEach(row => {

            const text = row.innerText.toLowerCase();

            row.style.display = text.includes(value) ? "" : "none";

        });
    });
}


// ================================
// LOGIN
// ================================

const loginForm = document.getElementById("loginForm");

if (loginForm) {

    loginForm.addEventListener("submit", function(e) {

        e.preventDefault();

        const email = document.querySelector("#loginForm input[type='email']").value.trim();

        const password = document.querySelector("#loginForm input[type='password']").value.trim();


        if (email === "" || password === "") {

            Swal.fire({
                title: "Missing Information",
                text: "Please enter your email and password.",
                icon: "error",
                confirmButtonColor: "#d33"
            });

            return;
        }


        // Demo login
        sessionStorage.setItem("loggedIn", "true");
        window.location.href = "index.html";
    });
}


// ================================
// SIGN UP
// ================================

const signupForm = document.getElementById("signupForm");

if (signupForm) {

    signupForm.addEventListener("submit", function(e) {

        e.preventDefault();

        const name = document.querySelector("#signupForm input[type='text']").value.trim();

        const email = document.querySelector("#signupForm input[type='email']").value.trim();

        const password = document.querySelector("#signupForm input[type='password']").value.trim();


        if (name === "" || email === "" || password === "") {

            Swal.fire({
                title: "Missing Information",
                text: "Please complete all signup fields.",
                icon: "error",
                confirmButtonColor: "#d33"
            });

            return;
        }


        // Demo signup
        sessionStorage.setItem("loggedIn", "true");
        window.location.href = "index.html";
    });
}

// ================================
// LOGOUT
// ================================

const logoutButton = document.getElementById("logoutButton");

if (logoutButton) {

    logoutButton.addEventListener("click", function() {

        sessionStorage.removeItem("loggedIn");

        window.location.href = "login.html";
    });
}

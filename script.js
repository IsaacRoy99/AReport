const reportForm = document.getElementById("reportForm");

function getStatusBadge(status){
    if(status === "Completed") return "success";
    if(status === "Blocked") return "danger";
    if(status === "On Hold") return "warning";
    return "primary";
}

function loadReports(){
    let reports = JSON.parse(localStorage.getItem("reports")) || [];
    let table = document.getElementById("reportTable");

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

reportForm.addEventListener("submit", function(e){
    e.preventDefault();

    const title = document.querySelector('input[type="text"]').value.trim();
    const team = document.getElementById("team").value.trim();
    const status = document.getElementById("status").value.trim();
    const summary = document.querySelectorAll('textarea')[0].value.trim();
    const challenges = document.querySelectorAll('textarea')[1].value.trim();
    const progress = document.querySelectorAll('textarea')[2].value.trim();

    if(
        title === "" ||
        team === "" ||
        status === "" ||
        summary === "" ||
        challenges === "" ||
        progress === ""
    ){
        Swal.fire({
            title: "Missing Information",
            text: "Please complete all required fields before submitting.",
            icon: "error"
        });
        return;
    }

    const report = {
        title,
        team,
        status,
        summary
    };

    let reports = JSON.parse(localStorage.getItem("reports")) || [];
    reports.push(report);

    localStorage.setItem("reports", JSON.stringify(reports));

    loadReports();

    Swal.fire({
        title: "Report Submitted!",
        text: "Your report has been saved successfully.",
        icon: "success",
        confirmButtonColor: "#fca311"
    });

    reportForm.reset();
});

loadReports();

const loginForm = document.getElementById("loginForm");

if(loginForm){
    loginForm.addEventListener("submit", function(e){
        e.preventDefault();

        window.location.href = "index.html";
    });
}

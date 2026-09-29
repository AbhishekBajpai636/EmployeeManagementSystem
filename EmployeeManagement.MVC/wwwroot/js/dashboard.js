$(document).ready(function () {

    loadDashboardEmployees();

});


function loadDashboardEmployees() {

    $.ajax({
        url: '/Employee/GetAll',
        type: 'GET',

        success: function (employees) {

            if (!employees) {
                showEmptyState();
                return;
            }

            updateStatistics(employees);

            bindRecentEmployees(employees);
        },

        error: function (xhr) {

            console.error('Dashboard API error:', xhr);

            $('#totalEmployees').text('0');
            $('#activeEmployees').text('0');
            $('#inactiveEmployees').text('0');

            $('#recentEmployeesBody').html(`
                <tr>
                    <td colspan="5"
                        class="text-center py-4 text-danger">
                        Unable to load employee data.
                    </td>
                </tr>
            `);
        }
    });
}


function updateStatistics(employees) {

    const total = employees.length;

    const active = employees.filter(function (employee) {
        return employee.isActive === true;
    }).length;

    const inactive = total - active;

    $('#totalEmployees').text(total);
    $('#activeEmployees').text(active);
    $('#inactiveEmployees').text(inactive);
}


function bindRecentEmployees(employees) {

    if (employees.length === 0) {
        showEmptyState();
        return;
    }

    // Temporary: latest 5 records
    const recentEmployees = employees.slice(0, 5);

    let html = '';

    recentEmployees.forEach(function (employee) {

        const statusHtml = employee.isActive
            ? `
                <span class="status-badge status-active">
                    <span class="status-dot"></span>
                    Active
                </span>
              `
            : `
                <span class="status-badge status-inactive">
                    <span class="status-dot"></span>
                    Inactive
                </span>
              `;

        html += `
            <tr>

                <td>
                    <span class="employee-name">
                        ${employee.name ?? '-'}
                    </span>
                </td>

                <td>
                    ${employee.email ?? '-'}
                </td>

                <td>
                    <span class="badge bg-primary-subtle text-primary">
                        ${employee.department ?? '-'}
                    </span>
                </td>

                <td>
                    ${employee.designation ?? '-'}
                </td>

                <td>
                    ${statusHtml}
                </td>

            </tr>
        `;
    });

    $('#recentEmployeesBody').html(html);
}


function showEmptyState() {

    $('#totalEmployees').text('0');
    $('#activeEmployees').text('0');
    $('#inactiveEmployees').text('0');

    $('#recentEmployeesBody').html(`
        <tr>
            <td colspan="5"
                class="text-center py-4 text-muted">
                No employees found.
            </td>
        </tr>
    `);
}
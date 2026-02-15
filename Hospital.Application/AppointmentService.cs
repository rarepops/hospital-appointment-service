using Microsoft.EntityFrameworkCore;

namespace Hospital.Application;

public class AppointmentService(AppointmentRepository appointmentRepository)
{
    public async Task<bool> ScheduleAppointment(
        string cpr, string patientName, DateTime appointmentDate,
        string department, string doctorName)
    {
        // Basic validation
        if (string.IsNullOrEmpty(cpr) || string.IsNullOrEmpty(department) ||
            string.IsNullOrEmpty(doctorName) || appointmentDate < DateTime.Now)
        {
            Console.WriteLine("[ERROR] Invalid appointment request.");
            return false;
        }

        // Validate CPR before scheduling
        if (!await new NationalRegistryService().ValidateCpr(cpr))
        {
            Console.WriteLine("[ERROR] Invalid CPR number. Cannot schedule appointment.");
            return false;
        }

        Console.WriteLine($"[LOG] Scheduling appointment for {patientName} (CPR: {cpr}) in {department} with {doctorName} on {appointmentDate}");

        // Department-specific rules
        if (department == "General Practice")
        {
            if (!IsAssignedToGP(cpr, doctorName))
            {
                Console.WriteLine("[ERROR] Patients can only book appointments with their assigned GP.");
                return false;
            }
        }
        else if (department == "Physiotherapy")
        {
            if (RequiresReferral(department) && !HasValidReferral(cpr, department))
            {
                Console.WriteLine("[ERROR] Physiotherapy requires a doctor's referral.");
                return false;
            }

            if (RequiresInsuranceApproval(department) && !HasValidInsuranceApproval(cpr, department))
            {
                Console.WriteLine("[ERROR] Physiotherapy requires valid insurance approval.");
                return false;
            }
        }

        else if (department == "Surgery")
        {
            if (RequiresReferral(department) && !HasValidReferral(cpr, department))
            {
                Console.WriteLine("[ERROR] Surgery requires a specialist referral.");
                return false;
            }
        }
        else if (department == "Radiology")
        {
            if (RequiresReferral(department) && !HasValidReferral(cpr, department))
            {
                Console.WriteLine("[ERROR] Radiology requires a doctor's referral.");
                return false;
            }

            if (RequiresFinancialApproval(department) && !HasValidFinancialApproval(cpr, department))
            {
                Console.WriteLine("[ERROR] Radiology procedures may require financial approval.");
                return false;
            }
        }
        else
        {
            Console.WriteLine($"[ERROR] Unsupported department: {department}");
            return false;
        }

        await appointmentRepository.AddAsync(new Appointment
        {
            Cpr = cpr,
            PatientName = patientName,
            AppointmentDate = appointmentDate,
            Department = department,
            DoctorName = doctorName
        });

        Console.WriteLine($"[LOG] Appointment successfully scheduled for {patientName} (CPR: {cpr})");
        return true;
    }

    private bool IsAssignedToGP(string cpr, string doctorName)
    {
        Console.WriteLine($"[LOG] Checking GP assignment for {cpr}");
        return true; // Dummy check (To be implemented later)
    }

    private bool CheckDoubleBooking(string cpr, DateTime appointmentDate)
    {
        Console.WriteLine($"[LOG] Checking for double booking for CPR {cpr}");
        return false; // Dummy check (To be replaced later)
    }

    private bool RequiresReferral(string department)
    {
        return department == "Surgery" || department == "Radiology" || department == "Physiotherapy";
    }

    private bool RequiresInsuranceApproval(string department)
    {
        return department == "Physiotherapy";
    }

    private bool RequiresFinancialApproval(string department)
    {
        return department == "Radiology";
    }

    private bool HasValidReferral(string cpr, string department)
    {
        Console.WriteLine($"[LOG] Checking if referral exists for CPR {cpr} in {department}");
        return true; // Dummy check (To be replaced later)
    }

    private bool HasValidInsuranceApproval(string cpr, string department)
    {
        Console.WriteLine($"[LOG] Checking if insurance approval exists for CPR {cpr} in {department}");
        return true; // Dummy check (To be replaced later)
    }

    private bool HasValidFinancialApproval(string cpr, string department)
    {
        Console.WriteLine($"[LOG] Checking if financial approval exists for CPR {cpr} in {department}");
        return true; // Dummy check (To be replaced later)
    }
}
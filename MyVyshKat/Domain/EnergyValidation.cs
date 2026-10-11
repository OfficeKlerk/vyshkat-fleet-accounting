namespace MyVyshKat.Domain;

//общая проверка расхода энергии для всех потребителей
internal static class EnergyValidation
{
    public static double EnsurePositive(double dailyEnergyKwh)
    {
        if (!double.IsFinite(dailyEnergyKwh) || dailyEnergyKwh <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyKwh), dailyEnergyKwh,
                "Daily energy consumption must be a positive number");
        }

        return dailyEnergyKwh;
    }
}

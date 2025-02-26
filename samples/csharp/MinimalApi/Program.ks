val builder = WebApplication.createBuilder(args)

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.services.addOpenApi()

val app = builder.build()

// Configure the HTTP request pipeline.
if app.environment.isDevelopment() {
  app.mapOpenApi()
}

app.useHttpsRedirection()

val summaries = [
  "Freezing", 
  "Bracing",
  "Chilly",
  "Cool",
  "Mild",
  "Warm",
  "Balmy", 
  "Hot",
  "Sweltering",
  "Scorching",
]

app.mapGet("/weatherforecast") {
  Enumerable
    .range(1, 5)
    .select { i -> WeatherForecast(
      DateOnly.fromDateTime(DateTime.now.addDays(i)),
      Random.shared.next(-20, 55),
      summaries[Random.shared.next(summaries.length)]
    )}
    .toArray()
}.withName("GetWeatherForecast")

app.run()

data class WeatherForecast(val date: DateOnly, val temperatureC: Int, val summary: String?) {  
  val temperatureF get() = 32 + (temperatureC / 0.5556).toInt()
}
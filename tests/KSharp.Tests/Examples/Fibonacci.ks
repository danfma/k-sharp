
fun fibonacci(n: Int): Int {
  if n <= 2 {
    return 1
  }
  
  return fibonacci(n - 1) + fibonacci(n - 2)
}

val n = 5
val result = fibonacci(n)

writeLine(result)

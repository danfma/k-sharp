fun factorial(n: Int): Int {
  if n <= 2 {
    return n
  }
  
  return n * factorial(n - 1)
}

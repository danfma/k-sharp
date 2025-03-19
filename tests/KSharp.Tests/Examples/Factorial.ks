fun factorial(n: Int32): Int32 {
  if n <= 2 {
    return n
  }
  
  return n * factorial(n - 1)
}

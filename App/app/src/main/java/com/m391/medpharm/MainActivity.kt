package com.m391.medpharm

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import com.m391.medpharm.ui.theme.MedPharmTheme
import kotlinx.coroutines.selects.whileSelect
import java.math.BigInteger

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MedPharmTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Greeting("Android")
                }
            }
        }
    }
}

@Composable
fun Greeting(name: String, modifier: Modifier = Modifier) {
    Text(
        text = "Hello $name!",
        modifier = modifier
    )
}
class ListNode(var `val`: Int) {
       var next: ListNode? = null
}
fun addTwoNumbers(l1: ListNode? , l2 : ListNode? ): ListNode? {
    var firstNum = String()
    var secondNum = String()
    var firstNumPointer = l1
    var secondNumPointer = l2
    while(firstNumPointer?.`val` != null){
        firstNum += firstNumPointer.`val`
        firstNumPointer = firstNumPointer.next
    }
    while (secondNumPointer?.`val` != null){
        secondNum +=   secondNumPointer.`val`
        secondNumPointer = secondNumPointer.next
    }
    val sum = (BigInteger(firstNum.reversed()) + BigInteger(secondNum.reversed())).toString().reversed()
    val sol  = ListNode(sum[0].digitToInt())
    var currentNode = sol
   for(i in 1..<sum.length){
        val node  = ListNode(sum[i].digitToInt())
        currentNode.next = node
        currentNode= node
    }
    return null
}
fun addStrings(num1: String, num2: String): String {
    val maxLength = maxOf(num1.length, num2.length)
    val result = IntArray(maxLength + 1)
    var carry = 0

    var i = num1.length - 1
    var j = num2.length - 1
    var k = result.size - 1

    while (i >= 0 || j >= 0 || carry != 0) {
        val digit1 = if (i >= 0) num1[i] - '0' else 0
        val digit2 = if (j >= 0) num2[j] - '0' else 0

        val sum = digit1 + digit2 + carry
        result[k] = sum % 10
        carry = sum / 10

        i--
        j--
        k--
    }

    val startIndex = result.indexOfFirst { it != 0 }
    return if (startIndex == -1) "0" else result.slice(startIndex until result.size).joinToString("")
}
@Preview(showBackground = true)
@Composable
fun GreetingPreview() {
    MedPharmTheme {
        Greeting("Android")
    }
}
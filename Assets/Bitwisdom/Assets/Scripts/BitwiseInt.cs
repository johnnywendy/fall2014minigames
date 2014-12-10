using UnityEngine;
using System.Collections;

namespace Bitwise
{
	public class BitwiseInt {
		
		public int value,
		size;
		
		public BitwiseInt(int value, int size){
			this.value = value;
			this.size = size;
		}
		
		public BitwiseInt(int value)
		{
			this.value = value;
		}
		
		public void Set (int value, int size){
			this.value = value;
			this.size = size;
		}
		
		public void SetValue (int value){
			this.value = value;
		}
		
		public void SetSize (int size)
		{
			this.size = size;
		}
		
		public static BitwiseInt operator & (BitwiseInt i1, BitwiseInt i2){
			int x = (int)Mathf.Pow ((float)2,(float)i1.size-1);
			int i = 0;
			
			while (x > 1) {
				i = ((i1.value / x) == 1 && (i2.value / x) == 1) ? i + x : i;
				i1.SetValue(i1.value % x);
				i2.SetValue (i2.value % x);
				x = x / 2;
			}
			
			i = (i1.value == 1 && i2.value == 1) ? i + 1 : i;


			return new BitwiseInt(i,i1.size);
		}
		
		public static BitwiseInt operator | (BitwiseInt i1, BitwiseInt i2){
			int x = (int)Mathf.Pow ((float)2,(float)i1.size-1);
			int i = 0;
			
			while (x > 1) {
				i = ((i1.value / x) == 1 || (i2.value / x) == 1)? i+x:i;
				i1.SetValue(i1.value % x);
				i2.SetValue(i2.value % x);
				x = x / 2;
			}
			
			i = (i1.value == 1 || i2.value == 1) ? i + 1 : i;
			
			return new BitwiseInt(i,i1.size);
		}
		
		public static BitwiseInt operator ^ (BitwiseInt i1, BitwiseInt i2){
			int x = (int)Mathf.Pow ((float)2,(float)i1.size-1);
			int i = 0;
			
			while (x > 1) {
				i = (((i1.value / x) == 1 || (i2.value / x) == 1) && !((i1.value / x) == 1 && (i2.value / x))? i:i+x;
				i1.SetValue(i1.value % x);
				i2.SetValue(i2.value % x);
				x = x / 2;
			}
			
			i = ((i1.value == 1 || i2.value == 1) && !(i1.value == 1 && i2.value == 1)) ? i : i + 1;
			
			return new BitwiseInt(i,i1.size);
		}
		
		public static BitwiseInt operator ~ (BitwiseInt i1){
			int x = (int)Mathf.Pow ((float)2,(float)i1.size-1);
			int i = 0;
			
			while (x > 1) {
				i = ((i1.value / x) == 1)? i:i+x;
				i1.SetValue (i1.value % x);
				x = x / 2;
			}
			
			i = (i1.value == 1) ? i : i + 1;
			
			return new BitwiseInt(i,i1.size);
		}
	}
}

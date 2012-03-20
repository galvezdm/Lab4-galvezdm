using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetMileageFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            String ZeroMileage = "Zero mileage, you're going nowhere";
            String NegativeMileage = "Neagive mileage, you're going backwards";
            String PositiveMileage = "Positive Mileage, you're doing it right";

            using (mocks.Record())
            {
                
                mockDatabase.getCarLocation(0);
                LastCall.Return(ZeroMileage);
                
                mockDatabase.getCarLocation(-45);
                LastCall.Return(NegativeMileage);

                mockDatabase.getCarLocation(80);
                LastCall.Return(PositiveMileage);
            }

            var target = new Car(10);

            target.Database = mockDatabase;

            String result;
            result = target.getCarLocation(0);
            Assert.AreEqual(result, ZeroMileage);
            result = target.getCarLocation(-45);
            Assert.AreEqual(result, NegativeMileage);
            result = target.getCarLocation(80);
            Assert.AreEqual(result, PositiveMileage);
        }

        [Test()]
        public void TestThatBMWHasCorrectPrice()
        {
            var target = ObjectMother.bmw();
            Assert.AreEqual(80, target.getBasePrice());
        }
	}
}

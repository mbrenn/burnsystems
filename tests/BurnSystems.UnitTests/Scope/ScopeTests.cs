using System;
using System.Linq;
using NUnit.Framework;
using BurnSystems.Scope;

namespace BurnSystems.UnitTests.Scope
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        public void TestNativeSimpleScope()
        {
            var contextSource = new ContextSource("TEST");
            contextSource.Add<int>(() => 12);
            contextSource.Add(() => "Test");

            using(var context = new Context(contextSource, "TEST")) {
                Assert.That(context.Name, Is.EqualTo("TEST"));
                Assert.That(context.Get<string>(), Is.EqualTo("Test"));
                Assert.That(context.Get<int>(), Is.EqualTo(12));
            }
        }
        
        [Test]
        public void TestReferenceSimpleScope()
        {
            var contextSource = new ContextSource("TEST");
            var box = new Box()
            {
                Height = 10,
                Width = 20
            };
            
            var line = new Line()
            {
                X1 = 4,
                X2 = 5,
                Y1 = 6,
                Y2 = 8
            };
            
            contextSource.Add(() => line);
            contextSource.Add(() => box);
            
            using(var context = new Context(contextSource, "TEST")) {
                var newLine = context.Get<Line>();
                var newBox = context.Get<Box>();
                
                Assert.That(newLine, Is.Not.Null);
                Assert.That(newBox, Is.Not.Null);
                
                Assert.That(newLine.X1, Is.EqualTo(4));
                Assert.That(newBox.Height, Is.EqualTo(10));
            }
        }
        
        [Test]
        public void TestSingleDisposable()
        {
            DisposeTester.Reset();
            var contextSource = new ContextSource("TEST");
            
            contextSource.Add(() => new DisposeTester());
            
            using(var context = new Context(contextSource, "TEST")) {
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(0));
                        
            using(var context = new Context(contextSource, "TEST")) {
                var tester = context.Get<DisposeTester>();
                Assert.That(tester, Is.Not.Null);
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(1));
                        
            using(var context = new Context(contextSource, "TEST")) {
                var tester = context.Get<DisposeTester>();
                var tester2 = context.Get<DisposeTester>();
                
                Assert.That(tester, Is.Not.Null);
                Assert.That(tester2, Is.Not.Null);
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(2));
        }
        
        [Test]
        public void TestMultiDisposable()
        {
            DisposeTester.Reset();
            var contextSource = new ContextSource("TEST");
            
            contextSource.Add(() => new DisposeTester());
            contextSource.Add(() => new DisposeTester1());
            
            using(var context = new Context(contextSource, "TEST")) {
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(0));
                        
            using(var context = new Context(contextSource, "TEST")) {
                var tester = context.Get<DisposeTester>();
                
                Assert.That(tester, Is.Not.Null);
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(1));
                        
            using(var context = new Context(contextSource, "TEST")) {
                var tester = context.Get<DisposeTester>();
                var tester2 = context.Get<DisposeTester1>();
                
                Assert.That(tester, Is.Not.Null);
                Assert.That(tester2, Is.Not.Null);
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(3));
                        
            using(var context = new Context(contextSource, "TEST")) {
                var testers = context.GetAll<DisposeTester>().ToList();
                
                Assert.That(testers, Is.Not.Null);
                Assert.That(testers.Count, Is.EqualTo(2));
            }
            
            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(5));
        }
        
        [Test()]
        public void TestEmbeddedSource()
        {
            var globalContextSource = new ContextSource("Global-Context");
            globalContextSource.Add(() => new Box()
                {
                    Height = 1,
                    Width = 1
                });
            globalContextSource.Add(() => new Line()
                {
                    X1 = 1,
                    X2 = 1
                });
            
            using(var globalContext = new Context(globalContextSource, "GLOBAL")) {
                
                var localContextSource = new ContextSource("Local-Context");
                localContextSource.Add(() => new Box()
                {
                    Height = 2,
                    Width = 2
                });

                using(var localContext = new NestedContext(globalContext, localContextSource, "LOCAL")) {
                    var localBox = localContext.Get<Box>();
                    Assert.That(localBox.Height, Is.EqualTo(2));

                    var localBoxes = localContext.GetAll<Box>();
                    Assert.That(localBoxes, Is.Not.Null);
                    Assert.That(localBoxes.Count(), Is.EqualTo(2));
                }

                var globalBox = globalContext.Get<Box>();
                Assert.That(globalBox.Height, Is.EqualTo(1));

                var globalBoxes = globalContext.GetAll<Box>();
                Assert.That(globalBoxes, Is.Not.Null);
                Assert.That(globalBoxes.Count(), Is.EqualTo(1));
            }
        }
        
        [Test()]
        public void TestEmbeddedDisposeSource()
        {
            DisposeTester.DisposeCount = 0;

            var globalContextSource = new ContextSource("Global-Context");
            globalContextSource.Add(() => new DisposeTester1());
            globalContextSource.Add(() => new DisposeTester3());
            
            using(var globalContext = new Context(globalContextSource, "GLOBAL")) {

                var localContextSource = new ContextSource("Local-Context");
                localContextSource.Add(() => new DisposeTester2());

                using(var localContext = new NestedContext(globalContext, localContextSource, "LOCAL")) {
                    var local1 = localContext.Get<DisposeTester2>();
                    var local2 = localContext.Get<DisposeTester1>();

                    Assert.That(local1, Is.Not.Null);
                    Assert.That(local2, Is.Not.Null);
                }

                Assert.That(DisposeTester.DisposeCount, Is.EqualTo(1));
                var local31 = globalContext.Get<DisposeTester1>();
                var local32 = globalContext.Get<DisposeTester2>();
                var local33 = globalContext.Get<DisposeTester3>();

                Assert.That(local31, Is.Not.Null);
                Assert.That(local32, Is.Null);
                Assert.That(local33, Is.Not.Null);
            }

            Assert.That(DisposeTester.DisposeCount, Is.EqualTo(3));
        }
            
        
        public class Box
        {
            public int Width {
                get;
                set;
            }
            
            public int Height {
                get;
                set;
            }
            
            public override string ToString()
			{
				return string.Format("[Box Width={0}, Height={1}]", Width, Height);
			}

        }
        
        public class Line
        {
            public int X1 {
                get;
                set;
            }
            
            public int X2 {
                get;
                set;
            }
            
            public int Y1 {
                get;
                set;
            }
            
            public int Y2 {
                get;
                set;
            }
        	
        	public override string ToString()
			{
				return string.Format("[Line X1={0}, X2={1}, Y1={2}, Y2={3}]", X1, X2, Y1, Y2);
			}

        }
        
        public class DisposeTester : IDisposable
        {
            public static int DisposeCount = 0;

            public static void Reset()
            {
                DisposeCount = 0;
            }
            
            #region IDisposable implementation
            public void Dispose()
            {
                DisposeCount++;
            }
            #endregion
        }
        
        public class DisposeTester1 : DisposeTester
        {
        
        }
        
        public class DisposeTester2 : DisposeTester
        {
        
        }
        
        public class DisposeTester3 : DisposeTester
        {
        
        }
    }
}


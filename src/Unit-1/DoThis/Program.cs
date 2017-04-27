using System;
﻿using Akka.Actor;
using Akka.Actor.Dsl;

namespace WinTail
{
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // create top-level actors within the actor system
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            IActorRef fileValidatorActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            Props consoleReaderProps = Props.Create(()=>new ConsoleReaderActor());
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");


            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            MyActorSystem.WhenTerminated.Wait();
        }


    }
}

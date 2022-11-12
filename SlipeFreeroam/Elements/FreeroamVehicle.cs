using System.Numerics;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;

namespace SlipeFreeroam.Elements;

public class FreeroamVehicle : Vehicle
{
    private readonly System.Timers.Timer destructionTimer;
    private readonly System.Timers.Timer idleTimer;

    public FreeroamVehicle(ushort model, Vector3 position)
        : base(model, position)
    {
        this.PedEntered += HandleEnter;
        this.PedLeft += HandleLeft;
        this.Blown += HandleBlown;

        this.destructionTimer = new(15000);
        this.destructionTimer.Elapsed += HandleDestructionTimerElapse;

        this.idleTimer = new(300000);
        this.idleTimer.Elapsed += HandleIdleTimerElapse;

        this.Destroyed += HandleDestroy;
    }

    private void HandleEnter(Element sender, VehicleEnteredEventsArgs e)
    {
        this.idleTimer.Stop();
        this.destructionTimer.Stop();
    }

    private void HandleLeft(Element sender, VehicleLeftEventArgs e)
    {
        if (this.Occupants.Count == 0 && !this.IsDestroyed)
        {
            this.idleTimer.Start();
        }
    }

    private void HandleIdleTimerElapse(object? sender, System.Timers.ElapsedEventArgs e)
    {
        this.Health = 0;

        if (!this.IsDestroyed)
            this.destructionTimer.Start();
    }

    private void HandleDestructionTimerElapse(object? sender, System.Timers.ElapsedEventArgs e)
    {
        this.Destroy();
    }

    private void HandleBlown(Element sender)
    {
        if (!this.IsDestroyed)
            this.destructionTimer.Start();
    }

    private void HandleDestroy(Element obj)
    {
        this.destructionTimer.Dispose();
        this.idleTimer.Dispose();
    }

}
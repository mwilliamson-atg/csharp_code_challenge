Often, disparate pieces of code need to be notified when an event occurs, however having tight bindings or listeners that are passed around can break encapsulation or simply clutter code.  Instead, a common practice is to have an Event Dispatch system that lets producers generate events, then route them to the interested consumers.

Your challenge is to write such a system, meeting the following criteria:

1. Multiple objects/functions should be able to respond to the same event.
1. Code that is interested in an event should be able to register and unregister itself at any point, without needing to pass a listener through a chain of uninterested objects/functions.
1. Callbacks should be tightly bound to the event they're interested in.  If `ObjectA` is only interested in `EventA`, `ObjectA` should not be notified when `EventB` occurs.

Once you have finished this, please include 
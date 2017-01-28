import { Component } from '@angular/core';

@Component({
    selector: 'counter',
    template: require('../templates/counter.html')
})
export class CounterComponent {
    public currentCount = 0;

    public incrementCounter() {
        this.currentCount++;
    }
}

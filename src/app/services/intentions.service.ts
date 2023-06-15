import { Injectable } from '@angular/core';

export interface Intention {
  id: number;
  forName: string;
  prayer: string;
  date?: string;
}

@Injectable({
  providedIn: 'root'
})
export class IntentionsService {
  public intentions: Intention[] = [
    {
      forName: 'Matt Chorsey',
      prayer: 'New event: Trip to Vegas',
      id: 0,
    },
    {
      forName: 'Lauren Ruthford',
      prayer: 'Long time no chat',
      date: '6:12 AM',
      id: 1,
    },
    {
      forName: 'Jordan Firth',
      prayer: 'Report Results',
      date: '4:55 AM',
      id: 2,
    },
    {
      forName: 'Bill Thomas',
      prayer: 'The situation',
      date: 'Yesterday',
      id: 3,
    },
    {
      forName: 'Joanne Pollan',
      prayer: 'Updated invitation: Swim lessons',
      id: 4,
    },
    {
      forName: 'Andrea Cornerston',
      prayer: 'Last minute ask',
      date: 'Yesterday',
      id: 5,
    },
    {
      forName: 'Moe Chamont',
      prayer: 'Family Calendar - Version 1',
      id: 6,
    },
    {
      forName: 'Kelly Richardson',
      prayer: 'Placeholder Headhots',
      date: 'Last Week',
      id: 7,
    }
  ];

  constructor() { }

  public getIntentions(): Intention[] {
    return this.intentions;
  }

  public getIntentionById(id: number): Intention {
    return this.intentions[id];
  }
}

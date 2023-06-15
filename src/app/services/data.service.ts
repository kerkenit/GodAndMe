import { Injectable } from '@angular/core';

export interface Message {
  fromName: string;
  subject: string;
  date: string;
  id: number;
  read: boolean;
}

export interface Intention {
  id: number;
  forName: string;
  prayer: string;
  date?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public messages: Message[] = [
    {
      fromName: 'Matt Chorsey',
      subject: 'New event: Trip to Vegas',
      date: '9:32 AM',
      id: 0,
      read: false
    },
    {
      fromName: 'Lauren Ruthford',
      subject: 'Long time no chat',
      date: '6:12 AM',
      id: 1,
      read: false
    },
    {
      fromName: 'Jordan Firth',
      subject: 'Report Results',
      date: '4:55 AM',
      id: 2,
      read: false
    },
    {
      fromName: 'Bill Thomas',
      subject: 'The situation',
      date: 'Yesterday',
      id: 3,
      read: false
    },
    {
      fromName: 'Joanne Pollan',
      subject: 'Updated invitation: Swim lessons',
      date: 'Yesterday',
      id: 4,
      read: false
    },
    {
      fromName: 'Andrea Cornerston',
      subject: 'Last minute ask',
      date: 'Yesterday',
      id: 5,
      read: false
    },
    {
      fromName: 'Moe Chamont',
      subject: 'Family Calendar - Version 1',
      date: 'Last Week',
      id: 6,
      read: false
    },
    {
      fromName: 'Kelly Richardson',
      subject: 'Placeholder Headhots',
      date: 'Last Week',
      id: 7,
      read: false
    }
  ];

  public intentions: Intention[] = [
    {
      forName: 'Matt Chorsey',
      prayer: 'New event: Trip to Vegas',
      date: '9:32 AM',
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
      date: 'Yesterday',
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
      date: 'Last Week',
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

  public getMessages(): Message[] {
    return this.messages;
  }

  public getMessageById(id: number): Message {
    return this.messages[id];
  }

  public getIntentions(): Intention[] {
    return this.intentions;
  }

  public getIntentionById(id: number): Intention {
    return this.intentions[id];
  }
}

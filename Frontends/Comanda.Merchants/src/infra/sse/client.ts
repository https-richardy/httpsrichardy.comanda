type SSEConfig = {
  url: string;
  token?: string;
  onMessage: (data: unknown) => void;
  onError?: (error: Event) => void;
  reconnectInterval?: number;
};

export class SSEClient {
  private eventSource: EventSource | null = null;
  private reconnectTimer: ReturnType<typeof setTimeout> | null = null;
  private isExplicitlyClosed = false;

  private config: SSEConfig;

  constructor(config: SSEConfig) {
    this.config = config;
  }

  public connect() {
    this.isExplicitlyClosed = false;

    const finalUrl = this.config.token
      ? `${this.config.url}?access_token=${this.config.token}`
      : this.config.url;

    try {
      this.eventSource = new EventSource(finalUrl);

      this.eventSource.onmessage = (event) => {
        try {
          const parsed = JSON.parse(event.data);

          this.config.onMessage(parsed);
        } catch {
          return;
        }
      };

      this.eventSource.onerror = (err) => {
        if (this.config.onError) this.config.onError(err);

        this.disconnect();
        this.scheduleReconnect();
      };
    } catch {
      this.scheduleReconnect();
    }
  }

  public disconnect() {
    if (this.eventSource) {
      this.eventSource.close();
      this.eventSource = null;
    }
  }

  public close() {
    this.isExplicitlyClosed = true;
    if (this.reconnectTimer) {
      clearTimeout(this.reconnectTimer);
      this.reconnectTimer = null;
    }
    this.disconnect();
  }

  private scheduleReconnect() {
    if (this.isExplicitlyClosed || this.reconnectTimer) return;

    const interval = this.config.reconnectInterval || 5000;

    this.reconnectTimer = setTimeout(() => {
      this.reconnectTimer = null;
      this.connect();
    }, interval);
  }
}

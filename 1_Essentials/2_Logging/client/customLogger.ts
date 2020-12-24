import { ILogger, LogLevel } from "@microsoft/signalr";

export class CustomLogger implements ILogger {
    log(logLevel: LogLevel, message: string) {
        // Use `message` and `logLevel` to record the log message to your own system
        console.log(`${logLevel} :: ${message}`);
    }
}
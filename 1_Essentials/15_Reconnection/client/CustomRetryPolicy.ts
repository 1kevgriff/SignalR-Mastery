import { RetryContext } from "@microsoft/signalr";

export default class CustomRetryPolicy implements signalR.IRetryPolicy {
    maxRetryAttempts = 0;

    nextRetryDelayInMilliseconds(retryContext: RetryContext): number {
        console.info(`Retry :: ${retryContext.retryReason}`);
        if (retryContext.previousRetryCount === 10) return null; // stawp!

        var nextRetry = retryContext.previousRetryCount * 1000 || 1000;
        console.log(`Retry in ${nextRetry} milliseconds`);
        return nextRetry;
    }

}
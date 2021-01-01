import Vue from "vue";
import { HubConnection } from "@microsoft/signalr"

declare module 'vue/types/vue'
{
    interface Vue {
        $connectionService: any;
    }

    interface VueConstructor {
        $connectionService: any;
    }

    interface ComponentOptions<V extends Vue> {
        $connectionService: any
    }
}
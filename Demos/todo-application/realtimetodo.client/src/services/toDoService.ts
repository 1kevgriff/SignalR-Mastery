import Vue from "vue";
import { EventEmitter } from "events";

import { PluginObject, VueConstructor } from "vue/types/umd";

import { LogLevel, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";

export default class ToDoService {

    connection: signalR.HubConnection;
    events: EventEmitter;

    constructor() {
        this.events = new EventEmitter();
        this.connection = new HubConnectionBuilder()
            .configureLogging(LogLevel.Trace)
            .withAutomaticReconnect()
            .withUrl("/hubs/todo")
            .build();

        this.connection.on("UpdatedToDoList", (values: any[]) =>
            this.events.emit("updatedToDoList", values));
        this.connection.on("UpdatedListData", (value: any) =>
            this.events.emit("updatedListData", value));
    }

    async start() {
        await this.connection.start();
    }

    getLists() {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("GetLists");
        }
        else {
            setTimeout(() => this.getLists(), 500);
        }
    }

    getListData(id: number) {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("GetList", id);
        }
        else {
            setTimeout(() => this.getListData(id), 500);
        }
    }

    subscribeToCountUpdates() {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("SubscribeToCountUpdates");
        }
        else {
            setTimeout(() => this.subscribeToCountUpdates(), 500);
        }
    }

    unsubscribeFromCountUpdates() {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("UnsubscribeFromCountUpdates");
        }
        else {
            setTimeout(() => this.unsubscribeFromCountUpdates(), 500);
        }
    }

    subscribeToListUpdates(id: number) {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("SubscribeToListUpdates", id);
        }
        else {
            setTimeout(() => this.subscribeToListUpdates(id), 500);
        }
    }

    unsubscribeFromListUpdates(id: number) {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("UnsubscribeFromListUpdates", id);
        }
        else {
            setTimeout(() => this.unsubscribeFromListUpdates(id), 500);
        }
    }

    addToDoItem(listId: number, text: string) {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("AddToDoItem", listId, text);
        }
        else {
            setTimeout(() => this.addToDoItem(listId, text), 500);
        }
    }

    toggleToDoItem(listId: number, itemId: number) {
        if (this.connection.state === HubConnectionState.Connected) {
            this.connection.send("ToggleToDoItem", listId, itemId);
        }
        else {
            setTimeout(() => this.toggleToDoItem(listId, itemId), 500);
        }
    }
}

export const ConnectionServices: PluginObject<any> = {
    install(Vue: VueConstructor<Vue>, option: any | undefined) {
        Vue.$connectionService = new ToDoService();
        Vue.prototype.$connectionService = Vue.$connectionService;
    }
}
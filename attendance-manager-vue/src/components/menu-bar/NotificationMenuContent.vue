<template>
  <v-card>
    <v-card-title> Notifications </v-card-title>
    <div v-if="notifications.length != 0">
      <v-list-item
        v-for="notification in notifications"
        :key="notification.notificationId"
        @click="onRead(notification)"
      >
        <v-list-item-icon class="mr-3">
          <v-icon :color="getIconColor(notification.priority)">{{
            getIcon(notification.priority)
          }}</v-icon>
        </v-list-item-icon>
        <v-list-item-content class="pt-1 pb-1">
          <v-list-item-title
            class="text-wrap"
            :class="!notification.isRead ? 'font-weight-bold' : ''"
            ><span v-html="notification.message"></span
          ></v-list-item-title>
          <v-list-item-subtitle>
            {{ notification.createdOn }}
          </v-list-item-subtitle>
        </v-list-item-content>
        <v-list-item-action>
          <v-btn @click.stop="onDelete(notification.notificationId)" icon>
            <v-icon color="red">mdi-close</v-icon>
          </v-btn>
        </v-list-item-action>
      </v-list-item>
    </div>
    <MessageComponent
      description="There is no notification."
      color="green"
      icon="mdi-information"
      fontWeight="bold"
      v-else
    />
  </v-card>
</template>

<script lang="ts">
import { NotificationViewModel } from "@/modules/notification";
import NotificationService from "@/services/notification.service";
import { NotificationPriority } from "@/shared/enums";
import Vue from "vue";
import MessageComponent from "../shared-components/MessageComponent.vue";
export default Vue.extend({
  name: "NotificationMenuContent",
  components: { MessageComponent },
  props: {
    notifications: {
      type: Array as () => NotificationViewModel[],
      required: true,
    },
  },
  methods: {
    getIcon: function (priority: NotificationPriority): string {
      let icon = "mdi-alert-box";
      if (priority == NotificationPriority.Info) {
        icon = "mdi-information";
      } else {
        if (priority == NotificationPriority.Warning) {
          icon = "mdi-alert-octagon";
        }
      }
      return icon;
    },
    getIconColor: function (priority: NotificationPriority): string {
      let iconColor = "red";
      if (priority == NotificationPriority.Info) {
        iconColor = "blue";
      } else {
        if (priority == NotificationPriority.Warning) {
          iconColor = "yellow";
        }
      }
      return iconColor;
    },
    onRead: async function (
      notification: NotificationViewModel
    ): Promise<void> {
      if (!notification.isRead) {
        const result = await NotificationService.readMessageAsync(
          notification.notificationId
        );
        if (result) {
          this.$emit("read-notification", notification.notificationId);
        }
      }
    },
    onDelete: async function (notificationId: number): Promise<void> {
      const result = await NotificationService.removeMessageAsync(
        notificationId
      );
      if (result) {
        this.$emit("delete-notification", notificationId);
      }
    },
  },
});
</script>
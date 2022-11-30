<template>
  <v-container v-if="organizations.length > 0">
    <v-card class="orange lighten-3">
      <v-col>
        <v-treeview :items="organizations" activatable open-all>
          <template v-slot:prepend="{ item, open }">
            <v-icon v-if="item.children">
              {{ open ? "mdi-folder-open" : "mdi-folder" }}
            </v-icon>
            <v-icon v-else> mdi-file </v-icon>
          </template>
        </v-treeview>
      </v-col>
    </v-card>
  </v-container>
  <v-container v-else>
    <h2 class="">
      There is no department or specialization defined. Please add at least one
      department!
    </h2>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import StoreHelper from "@/store/store-helper";
import { OrganizationViewModel } from "@/modules/organization";
import { EventBus } from "@/main";
import { EVENT_BUS_RELOAD_ORGANIZATIONS } from "@/shared/constants";

export default Vue.extend({
  data() {
    return {
      // List with all the departments for the v-treeview component
      organizations: [] as OrganizationViewModel[],
    };
  },
  /**
   * Load all the organizations from API
   */
  async created() {
    this.organizations =
      await StoreHelper.organizationStore.loadOrganizations();
  },
  mounted: function () {
    /**
     * Emit an event using EventBus in order to update the treeview whenever the user add a new department or specialization
     */
    EventBus.$on(EVENT_BUS_RELOAD_ORGANIZATIONS, () => {
      this.organizations = StoreHelper.organizationStore.organizations;
      EventBus.$off(EVENT_BUS_RELOAD_ORGANIZATIONS);
    });
  },
});
</script>
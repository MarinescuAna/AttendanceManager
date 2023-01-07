<template>
  <v-container v-if="organizations.length > 0">
    <v-card class="orange lighten-3">
      <v-row class="ma-2">
        <!--Display the departments list into a card-->
        <v-col class="orange lighten-3">
          <v-list flat>
            <h2 class="ma-2">Departments and specializations</h2>
            <v-container class="overflow-y-auto" style="max-height: 600px">
              <v-list-item-group v-model="selectedItem">
                <v-list-item v-for="org in organizations" :key="org.name">
                  <v-list-item-content>
                    <v-list-item-title v-text="org.name"></v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list-item-group>
            </v-container>
          </v-list>
        </v-col>
        <!--Divide the departments and specializations-->
        <v-divider v-if="selectedItem != ''" vertical></v-divider>
        <!--Display the specializations list for each selected department-->
        <v-col v-if="selectedItem != ''">
          <v-scroll-y-transition mode="out-in">
            <v-card :key="selectedItem" flat>
              <v-card-title>
                <span class="text-h5 ma-4">
                  {{ selectedOrganization.name }}</span
                >
                <v-spacer></v-spacer>
                <!--Menu from the top-right corner-->
                <v-menu bottom left>
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn icon v-bind="attrs" v-on="on">
                      <v-icon>mdi-dots-vertical</v-icon>
                    </v-btn>
                  </template>
                  <v-list>
                    <!--Edit button-->
                    <v-row justify="center">
                      <v-dialog v-model="dialog" width="40%">
                        <template v-slot:activator="{ on, attrs }">
                          <v-list-item v-bind="attrs" v-on="on" link>
                            <v-list-item-title
                              ><v-icon>mdi-pencil</v-icon
                              >Edit</v-list-item-title
                            >
                          </v-list-item>
                        </template>
                        <ChangeDepartmentDialog
                          :departmentName="selectedOrganization.name"
                        />
                      </v-dialog>
                    </v-row>
                    <!--Delete button-->
                    <v-list-item
                      @click="onDeleteDepartment(selectedOrganization.id)"
                      link
                    >
                      <v-list-item-title
                        ><v-icon>mdi-delete</v-icon>Delete</v-list-item-title
                      >
                    </v-list-item>
                  </v-list>
                </v-menu>
              </v-card-title>
              <v-divider></v-divider>
              <v-card-text>
                <v-row justify="center">
                  <v-col>
                    <h4>Specializations:</h4>
                  </v-col>
                  <v-col>
                    <v-list-item-group
                      v-if="selectedOrganization?.children.length > 0"
                    >
                      <v-list-item
                        v-for="child in selectedOrganization.children"
                        :key="child.id"
                      >
                        <v-list-item-content>
                          <v-list-item-title
                            v-text="child.name"
                          ></v-list-item-title>
                        </v-list-item-content>
                      </v-list-item>
                    </v-list-item-group>
                    <p v-else>
                      This department has no specializations defined!
                    </p>
                  </v-col>
                </v-row>
              </v-card-text></v-card
            >
          </v-scroll-y-transition>
        </v-col>
      </v-row>
    </v-card>
  </v-container>
  <v-container v-else>
    <h2 class="">
      There is no department or specialization defined. Please add at least one
      department!
    </h2>
  </v-container>
</template>

<style lang="css" scoped>
.v-list-scroll {
  height: 200px;
  overflow-y: auto;
}
</style>

<script lang="ts">
import Vue from "vue";
import StoreHelper from "@/store/store-helper";
import { OrganizationViewModel } from "@/modules/organization";
import { EventBus } from "@/main";
import { EVENT_BUS_RELOAD_ORGANIZATIONS } from "@/shared/constants";
import { UpdateDepartmentModule } from "@/modules/organization/departments";
import ChangeDepartmentDialog from "./ChangeDepartmentDialog.vue";

export default Vue.extend({
  components: {
    ChangeDepartmentDialog,
  },
  data() {
    return {
      // List with all the departments for the v-treeview component
      organizations: [] as OrganizationViewModel[],
      // Selected organization id
      selectedItem: "",
      // Flag for Change department dialog
      dialog: false,
    };
  },
  /**
   * Load all the organizations from API
   */
  async created() {
    this.organizations =
      await StoreHelper.organizationStore.loadOrganizations();
  },
  computed: {
    /**
     * Get selected specialization
     */
    selectedOrganization(): OrganizationViewModel {
      return this.organizations[
        this.selectedItem == undefined ? 0 : this.selectedItem
      ];
    },
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
  methods: {
    /**
     * Delete department from db and store
     * @param departmentId 
     */
    async onDeleteDepartment(departmentId: string): Promise<void> {
      const response = await StoreHelper.organizationStore.removeDepartment({
        departmentId: departmentId,
      } as UpdateDepartmentModule);

      if (response.isSuccess) {
        this.organizations = StoreHelper.organizationStore.organizations;
      } else {
        window.alert(response.error);
      }
    },
  },
});
</script>
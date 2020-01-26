import React from 'react';
import { connect } from 'react-redux';
import { groupBy } from 'lodash';
import { Theme, createStyles, WithStyles, withStyles } from '@material-ui/core';
import Grid from '@material-ui/core/Grid';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import IconButton from '@material-ui/core/IconButton';
import InfoIcon from '@material-ui/icons/Info';

import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';

import { AppState } from '../../store/appState';
import { Pet } from '../../store/petsList/state';
import { requestPetsListData, requestPetsListFilteredData } from '../../store/petsList/actions';
import { Feature } from '../../store/featuresList/state';
import { requestFeaturesListData } from '../../store/featuresList/actions';

import { TakeCareModal } from '../../components/modals/takeCare/TakeCareModal';
import { CareSuccessModal } from '../../components/modals/careSuccessModal/CareSuccessModal';
import { UserInfoModalConnected } from '../../components/modals/userInfoModal/UserInfoModal';

const styles = (theme: Theme) => createStyles({
    root: {
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'space-around',
        overflow: 'hidden',
        backgroundColor: theme.palette.background.paper,
    },
    gridList: {
        // width: 500,
        // height: 450,
    },
    icon: {
        color: 'rgba(255, 255, 255, 0.54)',
    },
    formControl: {
        margin: theme.spacing(3),
    },
});

interface PetSearchProps extends WithStyles<typeof styles> {
    pets: Pet[];
    features: Feature[];
    loadPetsList: () => void;
    loadFeaturesList: () => void;
    loadPetsFilteredList: (ids: string[]) => void;
};

interface FeatureCheckbox {
    id: string;
    checked: boolean;
}

interface PetSearchState {
    features: FeatureCheckbox[];
    takeCareOpen: boolean;
    userInfoOpen: boolean;
    successCareOpen: boolean;
    petName: string;
};

const PetSearchPageStyled = withStyles(styles)(
    class PetSearchPage extends React.Component<PetSearchProps, PetSearchState> {
        constructor(props: PetSearchProps) {
            super(props);
            this.state = {
                features: [],
                takeCareOpen: false,
                userInfoOpen: false,
                successCareOpen: false,
                petName: ''
            };
        }

        componentDidMount() {
            this.props.loadPetsList();
            this.props.loadFeaturesList();
        }

        componentDidUpdate() {
            if (this.state.features.length !== this.props.features.length) {
                this.state = {
                    features: this.props.features.map(f => {
                        return {
                            id: f.petFeatureId,
                            checked: false,
                        };
                    }),
                    takeCareOpen: false,
                    userInfoOpen: false,
                    successCareOpen: false,
                    petName: ''
                };
            }
        }

        changeFeatureCheckbox = (id: string) => {
            const { features } = this.state;
            const feature = features.find(f => f.id === id);
            if (feature !== undefined) {
                const checked = !feature.checked;
                feature.checked = checked;
                this.setState({
                    features: features
                });
            }
            this.props.loadPetsFilteredList(this.state.features.filter(f => f.checked).map(f => f.id));
        }

        openPetDetails = (petName: string) => {
            this.setState({
                takeCareOpen: true,
                petName
            });
        }

        closePetDetails = () => {
            this.setState({
                takeCareOpen: false,
                petName: ''
            })
        }

        successPetDetails = () => {
            this.setState({
                takeCareOpen: false,
                userInfoOpen: true,
                petName: ''
            })
        }

        closeUserInfo = () => {
            this.setState({
                userInfoOpen: false
            })
        }

        successUserInfo = () => {
            this.setState({
                userInfoOpen: false,
                successCareOpen: true
            })
        }

        closeSuccessCare = () => {
            this.setState({
                successCareOpen: false
            })
        }

        successSuccessCare = () => {
            this.setState({
                successCareOpen: false
            })
        }

        render() {
            const { features } = this.props;
            const grouped = groupBy(features, f => f.category);

            const { pets, classes } = this.props;
            const { takeCareOpen, userInfoOpen, petName, successCareOpen } = this.state;
            return (
                <div className={classes.root}>
                    <Grid container spacing={3}>
                        <Grid item xs={2}>
                            <FormControl component="fieldset" className={classes.formControl}>
                                {Object.keys(grouped).map(key => {
                                    return (
                                        <>
                                            <FormLabel component="legend">{key}</FormLabel>
                                            <FormGroup>
                                                {grouped[key].map(f => {
                                                    const checked = this.state.features.find(feature => f.petFeatureId == feature.id)?.checked;
                                                    return (
                                                        <FormControlLabel key={`${f.petFeatureId}_${f.characteristic}`}
                                                            control={
                                                                <Checkbox
                                                                    onChange={() => this.changeFeatureCheckbox(f.petFeatureId)}
                                                                    key={`${f.petFeatureId}_${f.category}`}
                                                                    value={checked}
                                                                />
                                                            }
                                                            label={f.characteristic}
                                                        />);
                                                })}
                                            </FormGroup>
                                        </>
                                    );
                                })}
                            </FormControl>
                        </Grid>
                        <Grid item xs={10}>
                            <GridList cellHeight={180} className={classes.gridList} cols={4}>
                                {pets.map(pet => (
                                    <GridListTile key={pet.petId} onClick={() => this.openPetDetails(pet.name)}>
                                        <img src={pet.images[0].imagePath} alt={pet.name} />
                                        <GridListTileBar
                                            title={pet.name}
                                            subtitle={<span>{pet.description}</span>}
                                            actionIcon={
                                                <IconButton aria-label={`info about ${pet.name}`} className={classes.icon}>
                                                    <InfoIcon />
                                                </IconButton>
                                            }
                                        />
                                    </GridListTile>
                                ))}
                            </GridList>
                        </Grid>
                    </Grid>
                    <TakeCareModal 
                        open={takeCareOpen} 
                        petName={petName} 
                        handleClose={this.closePetDetails} 
                        handleSuccess={this.successPetDetails}
                    />
                    <UserInfoModalConnected 
                        open={userInfoOpen}
                        handleClose={this.closeUserInfo} 
                        handleSuccess={this.successUserInfo}
                    />
                    <CareSuccessModal 
                        open={successCareOpen} 
                        petName={petName} 
                        handleClose={this.closeSuccessCare} 
                        handleSuccess={this.successSuccessCare}
                    />
                </div>
            );
        }
    }
);

export const PetSearchPageConnected = connect(
    (appState: AppState) => ({
        pets: appState.pets.data,
        features: appState.features.data
    }),
    {
        loadPetsList: requestPetsListData,
        loadFeaturesList: requestFeaturesListData,
        loadPetsFilteredList: requestPetsListFilteredData
    }
)(PetSearchPageStyled);

import React from 'react';
import { connect } from 'react-redux';
import { groupBy } from 'lodash';
import { Theme, createStyles, WithStyles, withStyles } from '@material-ui/core';
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

const styles = (theme: Theme) => createStyles({
    root: {
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'space-around',
        overflow: 'hidden',
        backgroundColor: theme.palette.background.paper,
    },
    gridList: {
        width: 500,
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
};

const PetSearchPageStyled = withStyles(styles)(
    class PetSearchPage extends React.Component<PetSearchProps, PetSearchState> {
        constructor(props: PetSearchProps) {
            super(props);
            this.state = {
                features: []
            };
        }

        componentDidMount() {
            this.props.loadPetsList();
            this.props.loadFeaturesList();
        }

        componentDidUpdate(prevProps: PetSearchProps, prevState: PetSearchState) {
            if (this.state.features.length !== this.props.features.length) {
                this.state = {
                    features: this.props.features.map(f => {
                        return {
                            id: f.petFeatureId,
                            checked: false,
                        };
                    })
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

        render() {
            const { features } = this.props;
            const grouped = groupBy(features, f => f.category);

            const { pets, classes } = this.props;
            return (
                <div className={classes.root}>
                    <FormControl component="fieldset" className={classes.formControl}>
                        {Object.keys(grouped).map(key => {
                            return (
                                <>
                                    <FormLabel component="legend">{key}</FormLabel>
                                    <FormGroup>
                                        {grouped[key].map(f => {
                                            return (
                                                <FormControlLabel
                                                    control={
                                                        <Checkbox
                                                            checked={this.state.features.find(feature => f.petFeatureId == feature.id)?.checked}
                                                            onChange={() => this.changeFeatureCheckbox(f.petFeatureId)}
                                                            name={f.petFeatureId}
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
                    <GridList cellHeight={180} className={classes.gridList}>
                        {pets.map(pet => (
                            <GridListTile key={pet.petId}>
                                <img src={pet.images[0].imagePath} alt={pet.name} />
                                <GridListTileBar
                                    title={pet.name}
                                    subtitle={<span>by: {pet.description}</span>}
                                    actionIcon={
                                        <IconButton aria-label={`info about ${pet.name}`} className={classes.icon}>
                                            <InfoIcon />
                                        </IconButton>
                                    }
                                />
                            </GridListTile>
                        ))}
                    </GridList>
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
